using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using S_EDex365.API.Interfaces;
using S_EDex365.API.Models;
using S_EDex365.API.Services;

namespace S_EDex365.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherNotificationController : ControllerBase
    {
        private readonly ITeacherNotificationService _teacherService;
        private readonly IFirebaseNotificationService _notificationService;
        public TeacherNotificationController(ITeacherNotificationService teacherService, IFirebaseNotificationService notificationService)
        {
            _teacherService = teacherService;
            _notificationService = notificationService;
        }
        

        //[HttpGet("s/AllProblems/{userId}")]
        //public async Task<ActionResult<List<ProblemPostAll>>> GetProblemss(Guid userId)
        //{
        //    // Fetch all problem posts by the user
        //    var problemPosts = await _teacherService.GetAllPostByUserAsync(userId);

        //    // If no posts are found, return 404
        //    if (problemPosts.Count == 0)
        //        return NotFound();

        //    // Retrieve user's device token for sending notification
        //    var deviceToken = await _teacherService.GetUserTokenAsync(userId);

        //    if (!string.IsNullOrEmpty(deviceToken))
        //    {
        //        // Send notification to the user
        //        var notificationTitle = "New Problem Posts Available";
        //        var notificationBody = $"{problemPosts.Count} new problem posts have been added.";
        //        await _notificationService.SendNotificationAsync(notificationTitle, notificationBody, deviceToken,postId);
        //    }

        //    return Ok(problemPosts);
        //}
    }
}
