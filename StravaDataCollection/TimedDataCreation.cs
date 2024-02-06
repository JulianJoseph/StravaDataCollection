using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using System.Net;

namespace StravaDataCollection
{
    public class TimedDataCreation
    {
        private readonly ILogger<TimedDataCreation> _logger;

        public TimedDataCreation(ILogger<TimedDataCreation> logger)
        {
            _logger = logger;
        }


        [Function("TimedDataCreation")]
        public async Task Run([TimerTrigger("0 0 * * * *")] TimerInfo myTimer)
        {
            // Get the connection string from app settings and use it to create a connection.
            var str = Environment.GetEnvironmentVariable("sqldb_connection");
            using (SqlConnection conn = new SqlConnection(str))
            {
                int activityId = new Random().Next(1, 1000000);
                int elapsedTime = new Random().Next(1000, 20000);
                int movingTime = new Random().Next(1000, 20000);
                int distance = new Random().Next(10, 400);
                conn.Open();
                var text = $"INSERT dbo.Activity(ActivityID, ActivityDate, ActivityName, ActivityType, ElapsedTime, MovingTime, Distance) " +
                    $"VALUES ({activityId}, '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}', 'Test {activityId.ToString()}', 'Ride', {elapsedTime}, {movingTime}, {distance})";

                using (SqlCommand cmd = new SqlCommand(text, conn))
                {
                    // Execute the command and log the # rows affected.
                    try
                    {
                        var rows = await cmd.ExecuteNonQueryAsync();
                        _logger.LogInformation($"{rows.ToString()} rows were inserted");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "An error occurrred");
                    }
                    finally
                    {
                        _logger.LogInformation("Complete");
                    }
                }
            }
        }
    }
}
