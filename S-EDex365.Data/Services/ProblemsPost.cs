using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using S_EDex365.Data.Interfaces;
using S_EDex365.Model.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace S_EDex365.Data.Services
{
    public class ProblemsPost : IProblemsPost
    {
        private readonly string _connectionString;
        public ProblemsPost(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(_connectionString));
        }

        public async Task<List<Model.Model.ProblemsPost>> GetAllUserAsync()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var queryString = "select id,subject,";
                    queryString += "Topic,Class,Description,Photo,UserId";
                    var query = string.Format(queryString);
                    var problemsPostList = await connection.QueryAsync<Model.Model.ProblemsPost>(query);
                    var problemsPost = problemsPostList.ToList();
                    return problemsPost;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> InsertProblemsPostAsync(Model.Model.ProblemsPost problemsPost)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var queryString = "insert into ProblemsPost (id,subject,topic,class,Description,Photo,UserId,Status,GetDateby,Updateby) values ";
                    queryString += "( @id,@subject,@topic,@class,@Description,@Photo,@UserId,@Status,@GetDateby,@Updateby)";
                    var parameters = new DynamicParameters();
                    var problemsPostId = Guid.NewGuid().ToString();
                    parameters.Add("id", problemsPostId, DbType.String);
                    parameters.Add("subject", problemsPost.Subject, DbType.String);
                    parameters.Add("topic", problemsPost.Topic, DbType.String);
                    parameters.Add("class", problemsPost.Class, DbType.String);
                    parameters.Add("Description", problemsPost.Description, DbType.String);
                    parameters.Add("Photo", problemsPost.Photo, DbType.String);
                    parameters.Add("UserId", problemsPost.UserId, DbType.String);
                    parameters.Add("status", 1, DbType.Boolean);
                    parameters.Add("GetDateby", DateTime.Now.ToString("yyyy-MM-dd"));
                    parameters.Add("Updateby", DateTime.Now.ToString("yyyy-MM-dd"));
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
