using Dapper;
using FirebaseAdmin.Messaging;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using S_EDex365.API.Interfaces;
using S_EDex365.API.Models;

namespace S_EDex365.API.Services
{
    public class FirebaseNotificationService : IFirebaseNotificationService
    {
        private readonly string _connectionString;
        public FirebaseNotificationService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(_connectionString));
        }

        public async Task<string> GetAllInfo(Guid postId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "SELECT t1.Photo, t2.SubjectName, t1.Description FROM ProblemsPost t1 JOIN Subject t2 ON t1.SubjectId = t2.Id WHERE t1.Id = @Id";
                var parameters = new { Id = postId };

                var result = await connection.QueryFirstOrDefaultAsync<ProblemNotificationValue>(query, parameters);

                return result != null ? JsonConvert.SerializeObject(result) : "{}"; // Serialize result to JSON
            }
        }

        public async Task<string> SendNotificationAsync(string title, string body, string token, Guid postId, string photo, string subjectName, string description)
        {

            var baseUrl = "https://api.edex365.com/uploads/";

            var img = photo;
                if (!string.IsNullOrEmpty(img))
                {
                photo = baseUrl + img;
                }

            var type = "Teacher";
            var message = new Message()
            {
                Notification = new Notification()
                {
                    Title = title,
                    Body = body,
                    ImageUrl=photo
                },
                Data=new Dictionary<string, string> {
                    {"page","sobuj" },
                    {"problemPostId",""+ postId +"" },
                    {"subjectName",""+subjectName+"" },
                    {"description",""+description+"" },
                    {"type",""+type+"" }
                },
                Token = token // Device token
            };

            
            // Send a message to the device corresponding to the provided token.
            string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
            return response;
        }
    }
}
