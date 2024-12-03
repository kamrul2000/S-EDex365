using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using S_EDex365.Data.Interfaces;
using S_EDex365.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S_EDex365.Data.Services
{
    public class StudentDashboardService : IStudentDashboardService
    {
        private readonly string _connectionString;
        public StudentDashboardService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(_connectionString));
        }


        public async Task<string?> GetAllPendingProblemAsync(string userId)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var queryString = "SELECT COUNT(Id) FROM ProblemsPost WHERE Id NOT IN (SELECT ProblemPostId FROM SolutionPost) AND UserId = '" + userId + "' ";
                    var query = string.Format(queryString);
                    var ProblemList = await connection.ExecuteScalarAsync<int>(query);
                    return ProblemList.ToString();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<string?> GetAllSolutionAsync(Guid userId)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var queryString = "select count(Id) from SolutionPost where StudentId='" + userId + "' ";
                    var query = string.Format(queryString);
                    var ProblemList = await connection.ExecuteScalarAsync<int>(query);
                    return ProblemList.ToString();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<string?> GetAllTotalProblemAsync(Guid userId)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var queryString = "select count(Id) from ProblemsPost where UserId='" + userId + "' ";
                    var query = string.Format(queryString);
                    var ProblemList = await connection.ExecuteScalarAsync<int>(query);
                    return ProblemList.ToString();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
