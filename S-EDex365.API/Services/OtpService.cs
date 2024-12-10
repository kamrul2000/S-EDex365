using Dapper;
using Microsoft.Data.SqlClient;
using S_EDex365.API.Interfaces;
using S_EDex365.API.Models;
using S_EDex365.Model.Model;
using System.Data;

namespace S_EDex365.API.Services
{
    public class OtpService:IOtpService
    {
        private readonly string _connectionString;
        public OtpService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(_connectionString));
        }

        public async Task<UserResponse> UpdateOtpAsync(OtpDto otpDto)
        {
            if (otpDto == null) throw new ArgumentNullException(nameof(otpDto));
            if (otpDto.Id == null) throw new ArgumentNullException(nameof(otpDto.Id));
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    //var queryString = "Select otp from Users where id=@id";
                    //var parameters = new DynamicParameters();
                    //parameters.Add("id", user.Id.ToString(), DbType.String);

                    //var otpCode = await connection.QueryAsync<Guid>(queryString, parameters);

                    string queryString = "SELECT otp FROM Users WHERE id = @id";
                    var parameters = new DynamicParameters();
                    parameters.Add("id", otpDto.Id, DbType.Guid);

                    var otpCode = await connection.QuerySingleOrDefaultAsync<int>(queryString, parameters);

                    if (otpCode == int.Parse(otpDto.Otp))
                    {
                        queryString = "update Users set status=@status where id=@id";
                        parameters = new DynamicParameters();
                        parameters.Add("status", 1, DbType.Boolean);
                        parameters.Add("id", otpDto.Id, DbType.Guid);
                        var success = await connection.ExecuteAsync(queryString, parameters);
                    }
                    UserResponse userRes = new UserResponse();
                    userRes.Id = otpDto.Id;
                    userRes.OTP = otpDto.Otp;
                    return userRes;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }




    }
}
