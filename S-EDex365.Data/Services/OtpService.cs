using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using S_EDex365.Data.Interfaces;
using S_EDex365.Model.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S_EDex365.Data.Services
{
    public class OtpService : IOtpService
    {
        private readonly string _connectionString;
        public OtpService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(_connectionString));
        }

        public async Task<User> GetUserByIdAsync(Guid userId)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var queryString = "select * from Users where id='{0}' order by name  ";
                    var query = string.Format(queryString, userId);
                    var user = await connection.QueryFirstOrDefaultAsync<User>(query);
                    return user;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> UpdateOtpAsync(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (user.Id == null) throw new ArgumentNullException(nameof(user.Id));
            if (user.OTP == null) throw new ArgumentNullException(nameof(user.OTP));

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
                    parameters.Add("id", user.Id, DbType.Guid);

                    var otpCode = await connection.QuerySingleOrDefaultAsync<int>(queryString, parameters);

                    if (otpCode == int.Parse(user.OTP))
                    {
                        queryString = "update Users set status=@status where id=@id";
                        parameters = new DynamicParameters();
                        parameters.Add("status", 1, DbType.Boolean);
                        //parameters.Add("id", user.Id.ToString(), DbType.StringSystem.ArgumentNullException: ');
                        parameters.Add("id", user.Id, DbType.Guid);
                        var success = await connection.ExecuteAsync(queryString, parameters);

                        if (success > 0)
                        {
                            return true;
                        }
                    }

                    
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
