using Dapper;
using Microsoft.Data.SqlClient;
using S_EDex365.API.Interfaces;
using S_EDex365.API.Models;

namespace S_EDex365.API.Services
{
    public class EnglishClassService:IEnglishClassService
    {
        private readonly string _connectionString;
        public EnglishClassService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<EnglishClassResponse>> GetAllEnglishClassAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var queryString = "select id,ClassName from EnglishMediumClass where Status=1 ";
                var query = string.Format(queryString);
                var classList = await connection.QueryAsync<EnglishClassResponse>(query);
                connection.Close();
                return classList.ToList();
            }
        }
    }
}
