using Dapper;
using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using S_EDex365.API.Interfaces;
using S_EDex365.API.Models;
using S_EDex365.API.Services;

namespace S_EDex365.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProblemsPostController : ControllerBase
    {
        private readonly IProblemsPost _problemsPost;
        private readonly ITeacherNotificationService _teacherService;
        private readonly IFirebaseNotificationService _notificationService;
        private readonly string _connectionString;
        public ProblemsPostController(IProblemsPost problemsPost, ITeacherNotificationService teacherService, IFirebaseNotificationService notificationService, IConfiguration configuration)
        {
            _problemsPost = problemsPost;
            _teacherService = teacherService;
            _notificationService = notificationService;
            _connectionString = configuration.GetConnectionString("DefaultConnection");

        }


        [HttpPost("s/ProblemsPost")]
        public async Task<IActionResult> UploadProblemPost([FromForm] ProblemsPostDto problemsPost)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    //var queryExist = "select SubjectId from TeacherSkill where SubjectId= @SubjectId and Status=1";
                    //var count = await connection.ExecuteScalarAsync<int>(queryExist, new { SubjectId = problemsPost.Subject });
                    //if (count == 0)
                    //{
                    //    return Ok(new { message= "There has no teacher available for this subject" });
                    //}
                    var queryExist = "SELECT COUNT(*) FROM TeacherSkill WHERE SubjectId = @SubjectId AND Status = 1";
                    var count = await connection.ExecuteScalarAsync<int>(queryExist, new { SubjectId = problemsPost.Subject });

                    if (count == 0)
                    {
                        return Ok(new { message = "There has no teacher available for this subject" });
                    }

                }


                if (problemsPost == null || problemsPost.UserId == Guid.Empty)
                {
                    return BadRequest("Invalid input data.");
                }

                // Insert the problem post and get the postId and response
                var (postId, problemsPostResponse) = await _problemsPost.InsertProblemPostAsync(problemsPost);

                if (postId == Guid.Empty)
                {
                    return BadRequest("Failed to insert the problem post.");
                }

                // Retrieve all posts by the user
                var problemPosts = await _teacherService.GetStudentProblemAsync(postId);

                

                // Retrieve the userIds based on postId
                var (devicePostId, userIds) = await _teacherService.GetStudentProblemAsync(postId);

                if (userIds == null || !userIds.Any())
                {
                    return NotFound("No users found for the given SubjectId.");
                }

                var problemList = await _notificationService.GetAllInfo(postId);
                var problemDetails = JsonConvert.DeserializeObject<ProblemNotificationValue>(problemList);
                // Use foreach to iterate over userIds and process each user
                foreach (var userIdItem in userIds)
                {
                    // Fetch the device token for each userId
                    var deviceToken = await _teacherService.GetUserTokenAsync(userIdItem);

                    if (!string.IsNullOrEmpty(deviceToken))
                    {
                        // Send a notification to the user
                        var notificationTitle = "Welcome";
                        var notificationBody = "New Task Added";
                        await _notificationService.SendNotificationAsync(notificationTitle, notificationBody, deviceToken, postId, problemDetails.Photo, problemDetails.SubjectName, problemDetails.Description);
                       
                    }
                    
                }
                

                // Return the result
                return Ok(new
                {
                    Message = "Problem post uploaded successfully.",
                    InsertedPost = problemsPostResponse,
                    //UserPosts = problemPosts
                });
            }
            catch (Exception ex)
            {
                // Log the exception (implement proper logging)
                Console.WriteLine($"Error in UploadProblemPost: {ex.Message}");
                return StatusCode(500, "An internal server error occurred.");
            }
        }






        [HttpGet("s/AllProblemsPost/{userId}")]
        public async Task<ActionResult<List<ProblemPostAll>>> GetAllProblems(Guid userId)
        {
            var problemPostDetails = await _problemsPost.GetAllUserAsync(userId);
            if (problemPostDetails.Count == 0)
                return Ok("No problem posts found for the specified user");
            return Ok(problemPostDetails);
        }
        [HttpGet("s/AllPendingPost/{userId}")]
        public async Task<ActionResult<List<ProblemPostAll>>> GetAllPending(Guid userId)
        {
            var problemPostDetails = await _problemsPost.GetPendingUserAsync(userId);
            if (problemPostDetails.Count == 0)
                return Ok("No problem posts found for the specified user");
            return Ok(problemPostDetails);
        }
        [HttpGet("s/AllSolutionsPost/{userId}")]
        public async Task<ActionResult<List<ProblemPostAll>>> GetAllSolution(Guid userId)
        {
            var problemPostDetails = await _problemsPost.GetSolutionUserAsync(userId);
            if (problemPostDetails.Count == 0)
                return Ok("No problem posts found for the specified user");
            return Ok(problemPostDetails);
        }

        [HttpGet("s/ProblemDetails/{postId}")]
        public async Task<ActionResult<ProblemPostAll>> ProblemDetails(Guid postId)
        {
            var problemPostDetail = await _problemsPost.GetPostDetailsUserAsync(postId);

            if (problemPostDetail == null)
                return Ok("No problem post found for the specified user");

            return Ok(problemPostDetail);
        }
    }
}
