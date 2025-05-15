using Dapper;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System;
using System.Threading;
using System.Threading.Tasks;
using S_EDex365.Model.Model;
using System.Data;

public class PaymentPeriodicTaskService : BackgroundService
{
    private readonly IConfiguration _configuration;

    public PaymentPeriodicTaskService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");

                using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync(stoppingToken); // ✅ open connection

                var timeNow = DateTime.Now.ToString("HH:mm");
                var query = @"SELECT ProblemPostId FROM SolutionPost 
                          WHERE CONVERT(varchar(5), PaymentTime, 108) = @Time and PaymentBlock=1";

                var result = await connection.QueryAsync<Guid>(query, new { Time = timeNow });

                if (!result.Any())
                {
                    Console.WriteLine("No problems found at this time.");
                }
                else
                {
                    foreach (var id in result)
                    {

                        // Validate if postId exists in the database
                        string queryString1 = "SELECT UserId FROM ProblemsPost WHERE id = @id";
                        var parameters1 = new DynamicParameters();
                        parameters1.Add("id", id, DbType.Guid);

                        var userId = await connection.QuerySingleOrDefaultAsync<Guid>(queryString1, parameters1);

                        if (userId == Guid.Empty)
                        {
                            throw new Exception("Invalid ProblemPostId.");
                        }

                        //var querys = "UPDATE RecivedProblem SET SolutionPending = 0,S_LastTime=NULL WHERE ProblemsPostId = @ProblemsPostId";
                        //await connection.ExecuteScalarAsync<Guid>(querys, new { ProblemsPostId = id });


                        //var queryProblemPost = "UPDATE ProblemsPost SET ForWallet = 1,TaskPending=0 WHERE Id = @Id";
                        //await connection.ExecuteScalarAsync<Guid>(queryProblemPost, new { Id = id });

                        var queryExisting = "SELECT Amount FROM Balance WHERE UserId = @UserId";
                        var parametersExisting = new DynamicParameters();
                        parametersExisting.Add("UserId", userId, DbType.Guid);

                        decimal existingAmount = await connection.QueryFirstOrDefaultAsync<decimal>(queryExisting, parametersExisting);


                        // 1. Get the existing amount
                        var queryProblemsPost = "SELECT Amount FROM ProblemsPost WHERE Id = @Id";
                        var parametersProblemsPost = new DynamicParameters();
                        parametersProblemsPost.Add("Id", id, DbType.Guid);

                        decimal existingProblemsPost = await connection.QueryFirstOrDefaultAsync<decimal>(queryProblemsPost, parametersProblemsPost);

                        // 2. Add the new amount to the existing one
                        decimal updatedAmount = existingAmount - existingProblemsPost;


                        var queryCheck = "SELECT COUNT(1) FROM SolutionPost WHERE ProblemPostId = @ProblemPostId";
                        var count = await connection.ExecuteScalarAsync<int>(queryCheck, new { ProblemPostId = id });

                        if (count > 0)
                        {
                            // 3. Update the Balance table with the new total
                            var queryBalance = "UPDATE Balance SET Amount = @Amount, GatDate = @GatDate WHERE UserId = @UserId";
                            var parametersBalance = new DynamicParameters();
                            parametersBalance.Add("UserId", userId);
                            parametersBalance.Add("Amount", updatedAmount);
                            parametersBalance.Add("GatDate", DateTime.Now.ToString("yyyy-MM-dd"));

                            var successs = await connection.ExecuteAsync(queryBalance, parametersBalance);

                            var queryId = "SELECT UserId FROM RecivedProblem WHERE ProblemsPostId = @ProblemsPostId";
                            var TeacherId = await connection.ExecuteScalarAsync<Guid>(queryId, new { ProblemsPostId = id });


                            var queryExistingAmountTeacher = "SELECT Amount FROM TeacherBalance WHERE UserId = @UserId";
                            var parametersExistingAmountTeacher = new DynamicParameters();
                            parametersExistingAmountTeacher.Add("UserId", TeacherId, DbType.Guid);

                            decimal existingAmountTeacher = await connection.QueryFirstOrDefaultAsync<decimal>(queryExistingAmountTeacher, parametersExistingAmountTeacher);

                            decimal updatedTeacherAmount = existingAmountTeacher + existingProblemsPost;

                            var queryUpdate = "UPDATE SolutionPost SET PaymentBlock = 0 WHERE ProblemPostId = @ProblemPostId";
                            await connection.ExecuteScalarAsync<Guid>(queryUpdate, new { ProblemPostId = id });


                            if (updatedTeacherAmount > 0)
                            {
                                var queryTeacherBalance = "insert into TeacherBalance (id,UserId,Amount,GatDate) values ";
                                queryTeacherBalance += "(@id,@UserId,@Amount,@GatDate)";
                                var parametersTeacherBalance = new DynamicParameters();
                                var IdBalance = Guid.NewGuid().ToString();
                                parametersTeacherBalance.Add("id", IdBalance, DbType.String);
                                parametersTeacherBalance.Add("UserId", TeacherId);
                                parametersTeacherBalance.Add("Amount", updatedTeacherAmount);
                                parametersTeacherBalance.Add("GatDate", DateTime.Now.ToString("yyyy-MM-dd"));
                                var successsTeacherBalance = await connection.ExecuteAsync(queryTeacherBalance, parametersTeacherBalance);
                            }
                            else
                            {
                                // 3. Update the Balance table with the new total
                                var queryTeacherBalanceUpdate = "UPDATE TeacherBalance SET Amount = @Amount, GatDate = @GatDate WHERE UserId = @UserId";
                                var parametersTeacherBalanceUpdate = new DynamicParameters();
                                parametersTeacherBalanceUpdate.Add("UserId", TeacherId);
                                parametersTeacherBalanceUpdate.Add("Amount", updatedAmount);
                                parametersTeacherBalanceUpdate.Add("GatDate", DateTime.Now.ToString("yyyy-MM-dd"));

                                var successsTeacherBalanceUpdate = await connection.ExecuteAsync(queryTeacherBalanceUpdate, parametersTeacherBalanceUpdate);
                            }
                        }


                    }

                    Console.WriteLine($"Updated {result.Count()} records at {timeNow}");
                }



            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ ERROR in background service: " + ex.Message);
                // Optional: log the full stack trace or send to logging service
            }

            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }
    }

}
