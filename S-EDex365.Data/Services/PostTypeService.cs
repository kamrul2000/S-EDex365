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
    public class PostTypeService : IPostTypeService
    {
        private readonly string _connectionString;
        public PostTypeService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(_connectionString));
        }
        public Task<bool> DeletePostTypeAsync(PostType postType)
        {
            throw new NotImplementedException();
        }

        public async Task<List<PostType>> GetAllPostTypeAsync()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var queryString = "select id,name,status,(case when status=1 then 'Active' else 'InActive' end) StatusName from PostType order by name  ";
                    var query = string.Format(queryString);
                    var roleList = await connection.QueryAsync<PostType>(query);
                    return roleList.ToList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<PostType> GetPostTypeByIdAsync(Guid postTypeId)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var queryString = "select * from PostType where id='{0}' order by name  ";
                    var query = string.Format(queryString, postTypeId);
                    var postType = await connection.QueryFirstOrDefaultAsync<PostType>(query);
                    return postType;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> InsertPostTypeAsync(PostType postType)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var queryString = "select name from PostType where lower(name)='{0}' ";
                    var query = string.Format(queryString, postType.Name.ToLower());
                    var roleObj = await connection.QueryFirstOrDefaultAsync<string>(query);
                    if (roleObj != null && roleObj.Length > 0)
                        return false;
                    queryString = "insert into PostType (id,name,status,GetDateby,Updateby) values ";
                    queryString += "( @id,@name,@status,@GetDateby,@Updateby)";
                    var parameters = new DynamicParameters();
                    parameters.Add("id", Guid.NewGuid().ToString(), DbType.String);
                    parameters.Add("name", postType.Name, DbType.String);
                    parameters.Add("status", postType.Status, DbType.Int32);
                    parameters.Add("GetDateby", DateTime.Now.ToString("yyyy-MM-dd"), DbType.String);
                    parameters.Add("Updateby", DateTime.Now.ToString("yyyy-MM-dd"), DbType.String);
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

        public async Task<bool> UpdatePostTypeAsync(PostType postType)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var queryString = "update PostType set name=@name,status=@status where id=@id";
                    var parameters = new DynamicParameters();
                    parameters.Add("name", postType.Name, DbType.String);
                    parameters.Add("status", postType.Status, DbType.Int32);
                    parameters.Add("id", postType.Id.ToString(), DbType.String);
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
