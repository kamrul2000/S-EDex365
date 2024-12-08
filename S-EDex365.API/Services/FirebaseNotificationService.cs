using FirebaseAdmin.Messaging;
using S_EDex365.API.Interfaces;

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
        public async Task<string> SendNotificationAsync(string title, string body, string token, Guid postId)
        {
            var message = new Message()
            {
                Notification = new Notification()
                {
                    Title = title,
                    Body = body
                },
                Data=new Dictionary<string, string> {
                    {"page","sobuj" },
                    {"problemPostId",""+ postId +"" }
                
                },
                Token = token // Device token
            };

            
            // Send a message to the device corresponding to the provided token.
            string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
            return response;
        }
    }
}
