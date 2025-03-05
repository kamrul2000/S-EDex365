using Dapper;
using Microsoft.Data.SqlClient;
using S_EDex365.API.Interfaces;
using S_EDex365.API.Models;
using S_EDex365.API.Models.Payment;
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
                    parameters.Add("GetDateby", DateTime.Now.ToString("yyyy-MM-dd"));
                    parameters.Add("Updateby", DateTime.Now.ToString("yyyy-MM-dd"));
                    var success = await connection.ExecuteAsync(queryString, parameters);
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
