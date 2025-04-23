using Dapper;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System;
using System.Threading;
using System.Threading.Tasks;

public class PeriodicTaskService : BackgroundService
{
    private readonly IConfiguration _configuration;

    public PeriodicTaskService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");

                using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync(stoppingToken); // ✅ open connection

                var timeNow = DateTime.Now.ToString("HH:mm");
                var query = @"SELECT ProblemsPostId FROM RecivedProblem 
                          WHERE CONVERT(varchar(5), S_LastTime, 108) = @Time";

                var result = await connection.QueryAsync<Guid>(query, new { Time = timeNow });

                if (!result.Any())
                {
                    Console.WriteLine("No problems found at this time.");
                }
                else
                {
                    foreach (var id in result)
                    {
                        // Update SolutionPending in RecivedProblem
                        var queryUpdate = "UPDATE RecivedProblem SET SolutionPending = 0 WHERE ProblemsPostId = @Id";
                        await connection.ExecuteAsync(queryUpdate, new { Id = id });

                        // Update Flag in ProblemsPost
                        var queryProblemPost = "UPDATE ProblemsPost SET Flag = 0 WHERE Id = @PostId";
                        await connection.ExecuteAsync(queryProblemPost, new { PostId = id }); // ✅ use ExecuteAsync
                    }

                    Console.WriteLine($"Updated {result.Count()} records at {timeNow}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ ERROR in background service: " + ex.Message);
                // Optional: log the full stack trace or send to logging service
            }

            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }
    }

}
