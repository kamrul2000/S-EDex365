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
    public class PostTypeDetailsService : IPostTypeDetailsService
    {
        private readonly string _connectionString;
        public PostTypeDetailsService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(_connectionString));
        }
        public async Task<bool> DeletePostTypeDetailsAsync(Guid postTypeDetailsId)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var queryString = "delete from PostTypeDetails where id=@id";
                    var parameters = new DynamicParameters();
                    parameters.Add("id", postTypeDetailsId.ToString(), DbType.String);
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

        public async Task<List<PostType>> GetAllPostTypeAsync()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var queryString = "select id,name from PostType order by name  ";
                    var query = string.Format(queryString);
                    var detailsList = await connection.QueryAsync<PostType>(query);
                    return detailsList.ToList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<PostTypeDetails>> GetAllPostTypeDetailsAsync()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var queryString = "select id,name,status,(case when status=1 then 'Active' else 'InActive' end) StatusName from PostTypeDetails order by name  ";
                    var query = string.Format(queryString);
                    var detailsList = await connection.QueryAsync<PostTypeDetails>(query);
                    return detailsList.ToList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<PostTypeDetails> GetPostTypeDetailsByIdAsync(Guid postTypeDetailsId)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var queryString = "select * from PostTypeDetails where id='{0}' order by name  ";
                    var query = string.Format(queryString, postTypeDetailsId);
                    var postTypeDetails = await connection.QueryFirstOrDefaultAsync<PostTypeDetails>(query);
                    return postTypeDetails;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> InsertPostTypeDetailsAsync(PostTypeDetails postTypeDetails)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var queryString = "select name from PostTypeDetails where lower(name)='{0}' ";
                    var query = string.Format(queryString, postTypeDetails.Name.ToLower());
                    var roleObj = await connection.QueryFirstOrDefaultAsync<string>(query);
                    if (roleObj != null && roleObj.Length > 0)
                        return false;
                    queryString = "insert into PostTypeDetails (id,name,PostTypeId,status,GetDateby,Updateby) values ";
                    queryString += "( @id,@name,@PostTypeId,@status,@GetDateby,@Updateby)";
                    var parameters = new DynamicParameters();
                    parameters.Add("id", Guid.NewGuid(), DbType.Guid);
                    parameters.Add("name", postTypeDetails.Name, DbType.String);
                    parameters.Add("PostTypeId", postTypeDetails.PostTypeId, DbType.Guid);
                    parameters.Add("status", postTypeDetails.Status, DbType.Int32);
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

        public async Task<bool> UpdatePostTypeDetailsAsync(PostTypeDetails postTypeDetails)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var queryString = "update PostTypeDetails set name=@name,status=@status,PostTypeId=@PostTypeId where id=@id";
                    var parameters = new DynamicParameters();
                    parameters.Add("name", postTypeDetails.Name, DbType.String);
                    parameters.Add("status", postTypeDetails.Status, DbType.Int32);
                    parameters.Add("id", postTypeDetails.Id.ToString(), DbType.String);
                    parameters.Add("PostTypeId", postTypeDetails.PostTypeId.ToString(), DbType.String);
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
