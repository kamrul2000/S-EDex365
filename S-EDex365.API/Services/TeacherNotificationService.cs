using Dapper;
using Microsoft.Data.SqlClient;
using S_EDex365.API.Interfaces;
using S_EDex365.API.Models;

namespace S_EDex365.API.Services
{
    public class TeacherNotificationService : ITeacherNotificationService
    {
        private readonly string _connectionString;
        private readonly string _basePhotoUrl = "http://192.168.0.238:81/uploads/";
        private readonly IFirebaseNotificationService _notificationService;
        public TeacherNotificationService(IConfiguration configuration, IFirebaseNotificationService notificationService)
        {
            _notificationService = notificationService;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<ProblemPostAll>> GetAllPostAsync()
        {
            throw new NotImplementedException();
        }
        public async Task<string> GetUserTokenAsync(Guid userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Query to fetch the DeviceToken based on userId
                var query = "SELECT DeviceToken FROM Users WHERE Id = @UserId";
                var deviceToken = await connection.QueryFirstOrDefaultAsync<string>(query, new { UserId = userId });

                return deviceToken; // Will return null if no DeviceToken is found
            }
        }
        //public async Task<List<ProblemPostAll>> GetAllPostByUserAsync(Guid userId)
        //{
        //    using (var connection = new SqlConnection(_connectionString))
        //    {
        //        await connection.OpenAsync();

        //        // Fetch the SubjectId based on userId
        //        var query = "SELECT CAST(SubjectId AS VARCHAR(36)) AS SubjectId FROM Users WHERE Id = @UserId";
        //        var subjectId = await connection.QueryFirstOrDefaultAsync<string>(query, new { UserId = userId });

        //        // Query to get the problem posts
        //        var queryString = @"SELECT t1.id, t1.Topic, t1.Description, t1.Photo, t2.SubjectName AS Subject, t3.ClassName AS sClass 
        //                    FROM ProblemsPost t1 
        //                    JOIN Subject t2 ON t2.Id = t1.SubjectId 
        //                    JOIN Class t3 ON t3.Id = t1.ClassId 
        //                    WHERE t1.SubjectId = @SubjectId";

        //        var parameters = new { SubjectId = subjectId };
        //        var problemPostList = (await connection.QueryAsync<ProblemPostAll>(queryString, parameters)).ToList();

        //        // Add base URL to the Photo property
        //        foreach (var post in problemPostList)
        //        {
        //            post.Photo = $"{_basePhotoUrl}{post.Photo}";
        //        }

        //        // Send notification if posts are found
        //        if (problemPostList.Count > 0)
        //        {
        //            var notificationTitle = "New Problem Posts Available";
        //            var notificationBody = $"{problemPostList.Count} new problem posts have been added.";

        //            // Retrieve user's notification token here if available (e.g., from database or request)
        //            string userToken = await GetUserTokenAsync(userId); // Assume this method fetches the token

        //            if (!string.IsNullOrEmpty(userToken))
        //            {
        //                await _notificationService.SendNotificationAsync(notificationTitle, notificationBody, userToken);
        //            }
        //        }

        //        return problemPostList;
        //    }
        //}

        public async Task<(Guid postId, List<Guid> userIds)> GetStudentProblemAsync(Guid postId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Fetch the SubjectId based on the postId
                var subjectIdQuery = "SELECT SubjectId FROM ProblemsPost WHERE Id = @Id";
                var subjectId = await connection.QueryFirstOrDefaultAsync<Guid?>(subjectIdQuery, new { Id = postId });

                if (subjectId == null)
                {
                    throw new Exception("SubjectId not found for the given postId.");
                }

                // Query to get the problem post details
                var userListQuery = "SELECT UserId FROM TeacherSkill WHERE SubjectId = @SubjectId";
                var userIds = await connection.QueryAsync<Guid>(userListQuery, new { SubjectId = subjectId });

                return (postId, userIds.ToList());

            }
        }

        public Task<List<ProblemPostAll>> GetAllPostByUserAsync(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
