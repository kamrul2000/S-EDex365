using Dapper;
using Microsoft.Data.SqlClient;
using S_EDex365.API.Interfaces;
using S_EDex365.API.Models;

namespace S_EDex365.API.Services
{
    public class CommunicationService : ICommunicationService
    {
        private readonly string _connectionString;
        public CommunicationService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<List<CommunicationResponse>> GetAllUserAsync(Guid userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var queryString = "select id,SubjectName from Communication where Status=1 ";
                var query = string.Format(queryString);
                var subjectList = await connection.QueryAsync<CommunicationResponse>(query);
                connection.Close();
                return subjectList.ToList();
            }
        }

        public Task<CommunicationResponse> InsertSubjectAsync(CommunicationResponse communicationResponse)
        {
            throw new NotImplementedException();
        }
    }
}
