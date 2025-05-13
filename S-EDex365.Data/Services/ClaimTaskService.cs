using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using S_EDex365.Data.Interfaces;
using S_EDex365.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S_EDex365.Data.Services
{
    public class ClaimTaskService : IClaimTaskService
    {
        private readonly string _connectionString;
        public ClaimTaskService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(_connectionString));
        }

        public async Task<List<ClaimTask>> GetAllClaimbyAsync()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var queryString = "  select t1.Id ,t2.Name as StudentName,t3.Name as TeacherName,t2.MobileNo as PhoneNumber,t2.Email as Email,t1.SolutionImage as SolutionImage,t1.TaskImage as TaskImage,t1.GetDateby as GetDateby  from ClaimTask t1 JOIN Users t2 on t1.StudentId=t2.Id JOIN Users t3 on t3.Id=t1.TeacherId order by t1.GetDateby";
                    var query = string.Format(queryString);
                    var claimTaskList = await connection.QueryAsync<ClaimTask>(query);
                    return claimTaskList.ToList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
