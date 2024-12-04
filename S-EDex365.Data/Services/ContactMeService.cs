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
    public class ContactMeService : IContactMeService
    {
        private readonly string _connectionString;
        public ContactMeService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(_connectionString));
        }
        public async Task<List<ContactMe>> GetAllContactMeAsync()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var queryString = "select Id,FullName,Email,Message from ContactMe where order by GetDateby  ";
                    var query = string.Format(queryString);
                    var contactmeList = await connection.QueryAsync<ContactMe>(query);
                    return contactmeList.ToList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> InsertContactMeAsync(ContactMe contactMe)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var queryString = "";
                    queryString = "insert into ContactMe (Id,FullName,Email,Message,GetDateby,Updateby) values ";
                    queryString += "( @Id,@FullName,@Email,@Message,@GetDateby,@Updateby)";
                    var parameters = new DynamicParameters();
                    parameters.Add("Id", Guid.NewGuid().ToString(), DbType.String);
                    parameters.Add("FullName", contactMe.FullName, DbType.String);
                    parameters.Add("Email", contactMe.Email, DbType.String);
                    parameters.Add("Message", contactMe.Message, DbType.String);
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
    }
}
