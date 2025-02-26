using Dapper;
using Microsoft.Data.SqlClient;
using S_EDex365.API.Interfaces;

namespace S_EDex365.API.Services
{
    public class StudentDashBoard : IStudentDashBoard
    {
        private readonly string _connectionString;
        public StudentDashBoard(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(_connectionString));
        }
        public async Task<int> GetAllPendingProblemAsync(Guid userId)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var queryString = "SELECT COUNT(Id) FROM ProblemsPost WHERE Id NOT IN (SELECT ProblemPostId FROM SolutionPost) AND UserId = '" + userId + "' ";
                    var query = string.Format(queryString);
                    var ProblemList = await connection.ExecuteScalarAsync<int>(query);
                    return ProblemList;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<int> GetAllSolutionAsync(Guid userId)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var queryString = "select * from SolutionPost t1 JOIN ProblemsPost t2 on t1.ProblemPostId=t2.Id JOIN Subject t3 on t3.Id=t2.SubjectId JOIN Class t4 ON t4.Id = t2.ClassId where StudentId='" + userId + "' ";
                    var query = string.Format(queryString);
                    var ProblemList = await connection.ExecuteScalarAsync<int>(query);
                    return ProblemList;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<int> GetAllTotalProblemAsync(Guid userId)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var queryString = "select count(Id) from ProblemsPost where UserId='" + userId + "' ";
                    var query = string.Format(queryString);
                    var ProblemList = await connection.ExecuteScalarAsync<int>(query);
                    return ProblemList;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
