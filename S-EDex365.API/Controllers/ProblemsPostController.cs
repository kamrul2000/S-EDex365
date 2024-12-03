using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public ProblemsPostController(IProblemsPost problemsPost, ITeacherNotificationService teacherService, IFirebaseNotificationService notificationService)
        {
            _problemsPost = problemsPost;
            _teacherService = teacherService;
            _notificationService = notificationService;

        }

        //[HttpPost("s/ProblemsPosts")]
        //public async Task<IActionResult> UploadProblemPosts([FromForm] ProblemsPostDto problemsPost, Guid userId)
        //{
        //    var result = await _problemsPost.InsertProblemPostAsync(problemsPost);
        //    var problemPosts = await _teacherService.GetAllPostByUserAsync(userId);
        //    //if (problemPosts.Count == 0)
        //    //    return NotFound();

        //    // Retrieve user's device token for sending notification
        //    var deviceToken = await _teacherService.GetStudentProblemAsync(postId);

        //    if (!string.IsNullOrEmpty(deviceToken))
        //    {
        //        // Send notification to the user
        //        var notificationTitle = "New Problem Posts Available";
        //        var notificationBody = $"{problemPosts.Count} new problem posts have been added.";
        //        await _notificationService.SendNotificationAsync(notificationTitle, notificationBody, deviceToken);
        //    }

        //    return Ok(problemPosts);
        //}

        //importn

        //[HttpPost("s/ProblemsPost")]
        //public async Task<IActionResult> UploadProblemPost([FromForm] ProblemsPostDto problemsPost, Guid userId)
        //{
        //    try
        //    {
        //        // Insert the problem post and get the postId and response
        //        var (postId, problemsPostResponse) = await _problemsPost.InsertProblemPostAsync(problemsPost);

        //        if (postId == Guid.Empty)
        //        {
        //            return BadRequest("Failed to insert the problem post.");
        //        }

        //        // Retrieve all posts by the user

        //        var problemPosts = await _teacherService.GetStudentProblemAsync(postId);

        //        //foreach (var userIdItem in problemPosts)
        //        //{
        //        //    // Fetch the device token for each userId
        //        //    var deviceToken = await _teacherService.GetDeviceTokenForUserAsync(userIdItem);

        //        //    if (!string.IsNullOrEmpty(deviceToken))
        //        //    {
        //        //        // Send a notification to the user
        //        //        var notificationTitle = "New Problem Post Added";
        //        //        var notificationBody = "A new problem post has been added successfully.";
        //        //        await _notificationService.SendNotificationAsync(notificationTitle, notificationBody, deviceToken);
        //        //    }
        //        //}


        //        // Return the result
        //        return Ok(new
        //        {
        //            Message = "Problem post uploaded successfully.",
        //            InsertedPost = problemsPostResponse,
        //            UserPosts = problemPosts
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception (implement proper logging)
        //        Console.WriteLine($"Error in UploadProblemPost: {ex.Message}");
        //        return StatusCode(500, "An internal server error occurred.");
        //    }
        //}


        [HttpPost("s/ProblemsPost")]
        public async Task<IActionResult> UploadProblemPost([FromForm] ProblemsPostDto problemsPost, Guid userId)
        {
            try
            {
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

                // Use foreach to iterate over userIds and process each user
                foreach (var userIdItem in userIds)
                {
                    // Fetch the device token for each userId
                    var deviceToken = await _teacherService.GetUserTokenAsync(userIdItem);

                    if (!string.IsNullOrEmpty(deviceToken))
                    {
                        // Send a notification to the user
                        var notificationTitle = "New Problem Post Added";
                        var notificationBody = "A new problem post has been added successfully.";
                        await _notificationService.SendNotificationAsync(notificationTitle, notificationBody, deviceToken);
                    }
                }

                // Return the result
                return Ok(new
                {
                    Message = "Problem post uploaded successfully.",
                    InsertedPost = problemsPostResponse,
                    UserPosts = problemPosts
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
        public async Task<ActionResult<List<ProblemPostAll>>> GetAllClass(Guid userId)
        {
            var problemPostDetails = await _problemsPost.GetAllUserAsync(userId);
            if (problemPostDetails.Count == 0)
                return NotFound("No problem posts found for the specified user");
            return Ok(problemPostDetails);
        }
    }
}
