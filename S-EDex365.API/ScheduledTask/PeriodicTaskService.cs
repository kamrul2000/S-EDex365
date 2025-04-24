using Dapper;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System;
using System.Threading;
using System.Threading.Tasks;
using S_EDex365.Model.Model;
using System.Data;

public class PeriodicTaskService : BackgroundService
{
    private readonly IConfiguration _configuration;

    public PeriodicTaskService(IConfiguration configuration)
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
                var query = @"SELECT ProblemsPostId FROM RecivedProblem 
                          WHERE CONVERT(varchar(5), S_LastTime, 108) = @Time";

                var result = await connection.QueryAsync<Guid>(query, new { Time = timeNow });

                if (!result.Any())
                {
                    Console.WriteLine("No problems found at this time.");
                }
                else
                {
                    foreach (var id in result)
                    {
                        var queryFlagCheck = "select flag from RecivedProblem where ProblemsPostId= @Id";
                        var check = await connection.ExecuteScalarAsync<int>(queryFlagCheck, new { Id = id });
                        if (check==0)
                        {
                            // Update SolutionPending in RecivedProblem
                            var queryUpdate = "UPDATE RecivedProblem SET SolutionPending = 0, Flag = 1, BlockFlag = 1, UnlockTime = @UnlockTime WHERE ProblemsPostId = @Id";
                            var parameters = new DynamicParameters();
                            parameters.Add("UnlockTime", DateTime.Now.AddMinutes(1440).ToString("HH:mm"), DbType.String);
                            parameters.Add("Id", id); // ← Add Id here
                            await connection.ExecuteAsync(queryUpdate, parameters);

                            //var queryFlag = "UPDATE RecivedProblem SET Flag = 1 WHERE ProblemsPostId = @Id";
                            //await connection.ExecuteAsync(queryFlag, new { Id = id });



                            // Update Flag in ProblemsPost
                            var queryProblemPost = "UPDATE ProblemsPost SET Flag = 0 WHERE Id = @PostId";
                            await connection.ExecuteAsync(queryProblemPost, new { PostId = id }); // ✅ use ExecuteAsync
                        }
                        
                    }

                    Console.WriteLine($"Updated {result.Count()} records at {timeNow}");
                }

                var timesNow = DateTime.Now.ToString("HH:mm");
                var querys = @"SELECT ProblemsPostId FROM RecivedProblem 
                          WHERE CONVERT(varchar(5), UnlockTime, 108) = @Time";

                var results = await connection.QueryAsync<Guid>(querys, new { Time = timesNow });
                if (!result.Any())
                {
                    Console.WriteLine("No problems found at this time.");
                }
                else
                {
                    foreach (var id in result)
                    {
                        var queryFlagCheck = "select flag from RecivedProblem where ProblemsPostId= @Id";
                        var check = await connection.ExecuteScalarAsync<int>(queryFlagCheck, new { Id = id });
                        if (check == 0)
                        {
                            // Update SolutionPending in RecivedProblem
                            var queryUpdate = "UPDATE RecivedProblem SET SolutionPending = 0, Flag = 1, BlockFlag = 1, UnlockTime = @UnlockTime WHERE ProblemsPostId = @Id";
                            var parameters = new DynamicParameters();
                            parameters.Add("UnlockTime", DateTime.Now.AddMinutes(1440).ToString("HH:mm"), DbType.String);
                            parameters.Add("Id", id); // ← Add Id here
                            await connection.ExecuteAsync(queryUpdate, parameters);

                            //var queryFlag = "UPDATE RecivedProblem SET Flag = 1 WHERE ProblemsPostId = @Id";
                            //await connection.ExecuteAsync(queryFlag, new { Id = id });



                            // Update Flag in ProblemsPost
                            var queryProblemPost = "UPDATE ProblemsPost SET Flag = 0 WHERE Id = @PostId";
                            await connection.ExecuteAsync(queryProblemPost, new { PostId = id }); // ✅ use ExecuteAsync
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
