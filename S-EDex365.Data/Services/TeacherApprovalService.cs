using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using S_EDex365.Data.Interfaces;
using S_EDex365.Model.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S_EDex365.Data.Services
{
    public class TeacherApprovalService : ITeacherApprovalService
    {
        private readonly string _connectionString;
        public TeacherApprovalService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(_connectionString));
        }
        public async Task<List<TeacherApproval>> GetAllTeacherApprovalListAsync()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var query1 = "select id from Roles where name='Teacher'";
                    var parameters = new DynamicParameters();
                    var Id = await connection.QueryFirstOrDefaultAsync<Guid>(query1);

                    var queryString = @"select Id,Name,MobileNo,Email from Users where Aproval=0 and UserTypeId='"+Id+"'";
                    var query = string.Format(queryString);
                    var approvalList = await connection.QueryAsync<TeacherApproval>(query);
                    return approvalList.ToList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //public async Task<TeacherApproval> GetTeacherApprovaByIdAsync(Guid userId)
        //{
        //    try
        //    {
        //        using (var connection = new SqlConnection(_connectionString))
        //        {
        //            connection.Open();
        //            var queryString = "select * from Users where id='{0}' order by name  ";
        //            var query = string.Format(queryString, userId);
        //            var teacherApproval = await connection.QueryFirstOrDefaultAsync<TeacherApproval>(query);
        //            return teacherApproval;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        public async Task<TeacherApproval> GetTeacherApprovaByIdAsync(Guid userId)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var query = "SELECT * FROM Users WHERE Id = @Id ORDER BY Name";
                    var teacherApproval = await connection.QueryFirstOrDefaultAsync<TeacherApproval>(query, new { Id = userId });
                    return teacherApproval;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public async Task<bool> UpdateTeacherApprovalAsync(TeacherApproval teacherApproval)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var queryString = "update Users set Aproval=@Aproval where id=@id";
                    var parameters = new DynamicParameters();
                    parameters.Add("Aproval", 1, DbType.String);
                    parameters.Add("id", teacherApproval.Id.ToString(), DbType.String);
                    var success = await connection.ExecuteAsync(queryString, parameters);
                    if (success > 0)
                    {
                        return true;
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
