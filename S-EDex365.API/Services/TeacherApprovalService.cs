using Dapper;
using Microsoft.Data.SqlClient;
using S_EDex365.API.Interfaces;
using S_EDex365.API.Models;
using System.Data;

namespace S_EDex365.API.Services
{
    public class TeacherApprovalService : ITeacherApprovalService
    {
        private readonly string _connectionString;
        public TeacherApprovalService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(_connectionString));
        }

        public async Task<List<TeacherApprovalResponse>> GetAllTeacherApprovalListAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var queryString = @"select Id,Name,MobileNo,Email from Users where Aproval=0";


                // Query the database using Dapper
                var userAllInformationList = await connection.QueryAsync<TeacherApprovalResponse>(queryString);

                // Map the result to the response DTO
                var teacherApprovalList = userAllInformationList.Select(user => new TeacherApprovalResponse
                {
                    Id=user.Id,
                    Name = user.Name,
                    MobileNo = user.MobileNo,
                    Email = user.Email
                }).ToList();

                return teacherApprovalList;
            }
        }


        public async Task<UserResponse> UpdateTeacherApprovalAsync(TeacherApprovalDto teacherApprovalDto)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    if (teacherApprovalDto.Aproval== true)
                    {
                        var queryString = "update Users set Aproval=@Aproval where id=@id";
                        var parameters = new DynamicParameters();
                        parameters.Add("Aproval", 1, DbType.Boolean);
                        parameters.Add("id", teacherApprovalDto.Id, DbType.Guid);
                        var success = await connection.ExecuteAsync(queryString, parameters);

                        UserResponse userRes = new UserResponse();
                        userRes.Id = teacherApprovalDto.Id;
                        return userRes;
                    }
                    else
                    {
                        var queryString = "delete from Users where id=@id";
                        var parameters = new DynamicParameters();
                        parameters.Add("id", teacherApprovalDto.Id, DbType.Guid);
                        var success = await connection.ExecuteAsync(queryString, parameters);

                        UserResponse userRes = new UserResponse();
                        userRes.Id = teacherApprovalDto.Id;
                        return userRes;
                    }

                        
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
