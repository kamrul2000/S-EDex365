using Dapper;
using Microsoft.Data.SqlClient;
using S_EDex365.API.Interfaces;
using S_EDex365.API.Models;
using S_EDex365.Model.Model;
using System.Data;

namespace S_EDex365.API.Services
{
    public class SubjectService : ISubjectService
    {
        private readonly string _connectionString;
        public SubjectService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<SubjectResponse>> GetAllSubjectAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var queryString = "select id,SubjectName from Subject where Status=1 ";
                var query = string.Format(queryString);
                var subjectList = await connection.QueryAsync<SubjectResponse>(query);
                connection.Close();
                return subjectList.ToList();
            }
        }

        public async Task<SubjectResponse> InsertSubjectAsync(SubjectResponse subjectResponse)
        {
            try
            {
                using (var connection=new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var queryString = "insert into Subject (id,SubjectName,status) values ";
                    queryString += "( @id,@SubjectName,@status)";
                    var parameters = new DynamicParameters();
                    var userId = Guid.NewGuid().ToString();
                    parameters.Add("id", userId, DbType.String);
                    parameters.Add("SubjectName", subjectResponse.SubjectName, DbType.String);
                    parameters.Add("status", 1, DbType.Boolean);
                    parameters.Add("UpdateAt", DateTime.Now.ToString("yyyy-MM-dd"));
                    var success = await connection.ExecuteAsync(queryString, parameters);

                    //if (success > 0)
                    //{
                    //    return new SubjectResponse();
                    //}
                    SubjectResponse s = new SubjectResponse();
                    s.Id = subjectResponse.Id;
                    s.SubjectName = subjectResponse.SubjectName;

                    return s;
                }
                
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
