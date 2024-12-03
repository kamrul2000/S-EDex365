using Dapper;
using Microsoft.Data.SqlClient;
using S_EDex365.API.Interfaces;
using S_EDex365.API.Models;

namespace S_EDex365.API.Services
{
    public class RoleService : IRoleService
    {
        private readonly string _connectionString;
        public RoleService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(_connectionString));
        }
        public async Task<List<RoleResponse>> GetRolesAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var queryString = "select id,name,(case when status=1 then 'Active' else 'InActive' end) StatusName from Roles where name<>'Super Admin' and name <> 'Admin' order by name ";
                var query = string.Format(queryString);
                var roleList = await connection.QueryAsync<RoleResponse>(query);
                connection.Close();
                return roleList.ToList();
            }
        }
    }
}
