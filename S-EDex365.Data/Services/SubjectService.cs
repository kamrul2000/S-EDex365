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

    public class SubjectService : ISubjectService
    {
        private readonly string _connectionString;
        public SubjectService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(_connectionString));
        }
        public async Task<bool> DeleteSubjectAsync(Guid subjectId)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    
                    var queryString = "delete from Subject where id=@id";
                    var parameters = new DynamicParameters();
                    parameters.Add("id", subjectId.ToString(), DbType.String);
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

        public async Task<List<Subject>> GetAllSubjectAsync()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var queryString = "Select SubjectName from Subject";
                    var query = string.Format(queryString);
                    var subjectList = await connection.QueryAsync<Subject>(query);
                    return subjectList.ToList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Subject> GetSubjectByIdAsync(Guid subjectId)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var queryString = "select * from Subject where id='{0}' ";
                    var query = string.Format(queryString, subjectId);
                    var subjectType = await connection.QueryFirstOrDefaultAsync<Subject>(query);
                    return subjectType;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> InsertSubjectAsync(Subject subject)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var queryString = "select SubjectName from Subject where lower(SubjectName)='{0}' ";
                    var query = string.Format(queryString, subject.SubjectName.ToLower());
                    var roleObj = await connection.QueryFirstOrDefaultAsync<string>(query);
                    if (roleObj != null && roleObj.Length > 0)
                        return false;
                    queryString = "insert into Subject (id,SubjectName,UpdateAt,Status) values ";
                    queryString += "( @id,@SubjectName,@UpdateAt,@Status)";
                    var parameters = new DynamicParameters();
                    parameters.Add("id", Guid.NewGuid().ToString(), DbType.String);
                    parameters.Add("SubjectName", subject.SubjectName, DbType.String);
                    parameters.Add("status", 1, DbType.Boolean);
                    parameters.Add("UpdateAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), DbType.String);
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

        public async Task<bool> UpdateSubjectsAsync(Subject subject)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var queryString = "update Subject set SubjectName=@SubjectName,UpdateAt=@UpdateAt where id=@id";
                    var parameters = new DynamicParameters();
                    parameters.Add("SubjectName", subject.SubjectName, DbType.String);
                    parameters.Add("UpdateAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), DbType.String);
                    parameters.Add("id", subject.Id.ToString(), DbType.String);
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
