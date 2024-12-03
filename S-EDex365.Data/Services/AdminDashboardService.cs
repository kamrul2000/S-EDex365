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
    public class AdminDashboardService: IAdminDashboardService
    {
        private readonly string _connectionString;
        public AdminDashboardService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(_connectionString));
        }

        public async Task<int> GetAllPendingAsync()
        {

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    // Query to get count of all SolutionPost records
                    var queryString = "select count(Id) from ProblemsPost";
                    var totalSolutions = await connection.ExecuteScalarAsync<int>(queryString);

                    // Query to get count of pending SolutionPost records (assuming status = 1 represents 'Pending')
                    var queryString1 = "select count(Id) from SolutionPost where status = 1";
                    var pendingSolutions = await connection.ExecuteScalarAsync<int>(queryString1);

                    // Calculate the difference between the total and pending solutions
                    var difference = totalSolutions - pendingSolutions;

                    // Prepare the result to return as a list of AdminDashboard
            //        var result = new List<AdminDashboard>
            //{
            //    new AdminDashboard
            //    {
            //        TotalSolutions = totalSolutions,
            //        PendingSolutions = pendingSolutions,
            //        Difference = difference
            //    }
            //};

                    return difference;
                }
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                throw;
            }



            
        }

        public async Task<int> GetAllSolutionAsync()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var queryString = "select count(Id) from SolutionPost";
                    var query = string.Format(queryString);
                    var SolutionList = await connection.ExecuteScalarAsync<int>(query);
                    return SolutionList;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        

        public async Task<int> GetAllTotalProblemAsync()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    
                    var queryString = "select count(Id) from ProblemsPost";

                    // Retrieve the count directly as an integer
                    var totalProblems = await connection.ExecuteScalarAsync<int>(queryString);
                    return totalProblems;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


    }
}
