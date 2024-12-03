using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using S_EDex365.API.Interfaces;
using S_EDex365.API.Models;

namespace S_EDex365.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly IFirebaseNotificationService _notificationService;
        public NotificationController(IFirebaseNotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        [HttpPost("s/send")]
        public async Task<IActionResult> SendNotification([FromBody] NotificationRequest request)
        {
            if (string.IsNullOrEmpty(request.Token))
            {
                return BadRequest("Device token is required");
            }

            var response = await _notificationService.SendNotificationAsync(request.Title, request.Body, request.Token);

            return Ok(new { MessageId = response });
        }
    }
}
