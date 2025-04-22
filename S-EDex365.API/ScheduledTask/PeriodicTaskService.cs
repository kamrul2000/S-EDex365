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
            var now = DateTime.Now;

            // Check if current time is 4:51 PM
            if (now.Hour == 17 && now.Minute == 11)
            {
                await RunQuery();
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken); // Avoid multiple runs in the same minute
            }

            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken); // Check every 10 seconds
        }
    }

    private async Task RunQuery()
    {
        var connectionString = _configuration.GetConnectionString("DefaultConnection");

        using var connection = new SqlConnection(connectionString);

        // how to collect User Id....
        //select UserId from RecivedProblem where SolutionPending = 0


        var query = "UPDATE ProblemsPost SET SolutionPending = 0 WHERE ProblemsPostId = 'DD0DF870-7AAE-4242-93A2-4A1221283703'";
        await connection.ExecuteAsync(query);
    }
}
