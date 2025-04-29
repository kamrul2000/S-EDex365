using Dapper;
using Microsoft.Data.SqlClient;
using S_EDex365.API.Interfaces;
using S_EDex365.API.Models;

namespace S_EDex365.API.Services
{
    public class PostTypeService : IPostTypeService
    {
        private readonly string _connectionString;
        public PostTypeService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<List<PostType>> GetAllPostTypeAsync()
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var queryString = "select id,Name as PostName from PostTypeDetails where Status=1 ";
                var query = string.Format(queryString);
                var classList = await connection.QueryAsync<PostType>(query);
                connection.Close();
                return classList.ToList();
            }
        }
    }
}
