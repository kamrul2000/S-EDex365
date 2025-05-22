using Dapper;
using Microsoft.Data.SqlClient;
using S_EDex365.API.Interfaces;
using S_EDex365.API.Models;
using S_EDex365.API.Models.Payment;
using S_EDex365.Model.Model;
using System.Data;

namespace S_EDex365.API.Services
{
    public class WalletService:IWalletService
    {
        private readonly string _connectionString;
        public WalletService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(_connectionString));
        }

        public async Task<List<StudentCostTransaction>> GetAllCostTransactionByAsync(Guid userId)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var query = @"
        SELECT t1.photo, t2.amount, t1.GetDateby 
        FROM SolutionPost t1 
        JOIN ProblemsPost t2 ON t1.ProblemPostId = t2.id 
        WHERE t1.StudentId = @StudentId";

                    var transactions = (await connection.QueryAsync<StudentCostTransaction>(query, new { StudentId = userId })).ToList();

                    var baseUrl = "https://api.edex365.com/solutionImage/";

                    foreach (var transaction in transactions)
                    {
                        if (!string.IsNullOrEmpty(transaction.Photo))
                        {
                            transaction.Photo = baseUrl + transaction.Photo;
                        }
                    }

                    return transactions;
                }


            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<TeacherTransaction>> GetAllTeacherTransactionByAsync(Guid userId)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var query = @"
        SELECT t1.photo, t2.amount, t1.GetDateby 
        FROM SolutionPost t1 
        JOIN ProblemsPost t2 ON t1.ProblemPostId = t2.id 
        WHERE t1.TeacherId = @StudentId";

                    var transactions = (await connection.QueryAsync<TeacherTransaction>(query, new { StudentId = userId })).ToList();

                    var baseUrl = "https://api.edex365.com/solutionImage/";

                    foreach (var transaction in transactions)
                    {
                        if (!string.IsNullOrEmpty(transaction.Photo))
                        {
                            transaction.Photo = baseUrl + transaction.Photo;
                        }
                    }

                    return transactions;
                }


            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<StudentTransaction>> GetAllTransactionByAsync(Guid userId)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var query = "SELECT TransactionId, Amount, GetDateby FROM Wallet WHERE UserId = @UserId";
                    var transInfoList = (await connection.QueryAsync<StudentTransaction>(query, new { UserId = userId })).ToList();

                    return transInfoList;
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<StudentWallet> GetStudentWalletAsync(Guid userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {

                var queryProblemsPost = "Select ISNULL(SUM(Amount), 0) from ProblemsPost where UserId=@UserId and ForWallet=0";
                var parametersProblemsPost = new DynamicParameters();
                parametersProblemsPost.Add("UserId", userId, DbType.Guid);

                decimal existingProblemsPost = await connection.QueryFirstOrDefaultAsync<decimal>(queryProblemsPost, parametersProblemsPost);

                


                var query = "SELECT ISNULL(SUM(Amount), 0) FROM Balance WHERE UserId = @UserId";
                var amount = await connection.ExecuteScalarAsync<decimal>(query, new { UserId = userId });

                decimal updatedAmount = amount - existingProblemsPost;

                return new StudentWallet { Balance = updatedAmount };
            }
        }

        public async Task<TeacherWallet> GetTeacherWalletAsync(Guid userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {

                var query = "SELECT ISNULL(SUM(Amount), 0) FROM TeacherBalance WHERE UserId = @UserId";
                var amount = await connection.ExecuteScalarAsync<decimal>(query, new { UserId = userId });

                return new TeacherWallet { Balance = amount };
            }
        }

        public async Task<bool> InsertWalletAsync(PaymentResponse paymentResponse, Guid userId)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var queryString = "insert into Wallet (id,UserId,TransactionId,Amount,GetDateby,Updateby) values ";
                    queryString += "(@id,@UserId,@TransactionId,@Amount,@GetDateby,@Updateby)";
                    var parameters = new DynamicParameters();
                    var Id = Guid.NewGuid().ToString();
                    parameters.Add("id", Id, DbType.String);
                    parameters.Add("UserId", userId);
                    parameters.Add("TransactionId", paymentResponse.PaymentID);
                    parameters.Add("Amount", paymentResponse.Amount);
                    parameters.Add("GetDateby", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    parameters.Add("Updateby", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    var success = await connection.ExecuteAsync(queryString, parameters);


                    var query = "SELECT COUNT(1) FROM Balance WHERE UserId = @UserId";
                    var count = await connection.ExecuteScalarAsync<int>(query, new { UserId = userId });

                    if (count <= 0)
                    {
                        var queryBalance = "insert into Balance (id,UserId,Amount,GatDate) values ";
                        queryBalance += "(@id,@UserId,@Amount,@GatDate)";
                        var parametersBalance = new DynamicParameters();
                        var IdBalance = Guid.NewGuid().ToString();
                        parametersBalance.Add("id", IdBalance, DbType.String);
                        parametersBalance.Add("UserId", userId);
                        parametersBalance.Add("Amount", paymentResponse.Amount);
                        parametersBalance.Add("GatDate", DateTime.Now.ToString("yyyy-MM-dd"));
                        var successs = await connection.ExecuteAsync(queryBalance, parametersBalance);
                    }
                    else
                    {
                        // 1. Get the existing amount
                        var queryExisting = "SELECT Amount FROM Balance WHERE UserId = @UserId";
                        var parametersExisting = new DynamicParameters();
                        parametersExisting.Add("UserId", userId, DbType.Guid);

                        decimal existingAmount = await connection.QueryFirstOrDefaultAsync<decimal>(queryExisting, parametersExisting);

                        // 2. Add the new amount to the existing one
                     decimal updatedAmount = existingAmount +
                    (!string.IsNullOrWhiteSpace(paymentResponse.Amount) && decimal.TryParse(paymentResponse.Amount, out var parsedAmount)
                        ? parsedAmount
                        : 0);

                        // 3. Update the Balance table with the new total
                        var queryBalance = "UPDATE Balance SET Amount = @Amount, GatDate = @GatDate WHERE UserId = @UserId";
                        var parametersBalance = new DynamicParameters();
                        parametersBalance.Add("UserId", userId);
                        parametersBalance.Add("Amount", updatedAmount);
                        parametersBalance.Add("GatDate", DateTime.Now.ToString("yyyy-MM-dd"));

                        var successs= await connection.ExecuteAsync(queryBalance, parametersBalance);

                    }


                    return true;
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
