using Dapper;
using Microsoft.Data.SqlClient;
using S_EDex365.API.Interfaces;
using S_EDex365.API.Models;
using System.Data;

namespace S_EDex365.API.Services
{
    public class ClassService:IClassService
    {
        private readonly string _connectionString;
        public ClassService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<ClassResponse>> GetAllClassAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var queryString = "select id,ClassName from Class where Status=1 ";
                var query = string.Format(queryString);
                var classList = await connection.QueryAsync<ClassResponse>(query);
                connection.Close();
                return classList.ToList();
            }
        }

        public async Task<ClassResponse> InsertClassAsync(ClassResponse classResponse)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var queryString = "insert into Class (id,ClassName,status) values ";
                    queryString += "( @id,@ClassName,@status)";
                    var parameters = new DynamicParameters();
                    var userId = Guid.NewGuid().ToString();
                    parameters.Add("id", userId, DbType.String);
                    parameters.Add("ClassName", classResponse.ClassName, DbType.String);
                    parameters.Add("status", 1, DbType.Boolean);
                    parameters.Add("UpdateAt", DateTime.Now.ToString("yyyy-MM-dd"));
                    var success = await connection.ExecuteAsync(queryString, parameters);

                    //if (success > 0)
                    //{

                    //}
                    ClassResponse c = new ClassResponse();
                    c.Id = classResponse.Id;
                    c.ClassName = classResponse.ClassName;
                    return c;
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
