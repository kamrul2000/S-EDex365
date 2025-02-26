using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using S_EDex365.API.Interfaces;
using S_EDex365.API.Models;
using S_EDex365.API.Services;

namespace S_EDex365.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherService _teacherService;
        public TeacherController(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }
        [HttpGet("s/AllProblems")]
        public async Task<ActionResult<List<ProblemPostAll>>> GetProblems()
        {
            var subjectDetails = await _teacherService.GetAllPostAsync();
            if (subjectDetails.Count == 0)
                return NotFound();
            return Ok(subjectDetails);
        }
        [HttpGet("s/AllProblems/{userId}")]
        public async Task<ActionResult<List<ProblemList>>> GetProblems(Guid userId)
        {
            var subjectDetails = await _teacherService.GetAllPostByUserAsync(userId);

            if (subjectDetails.Count == 0)
                return NotFound();

            return Ok(subjectDetails);
        }

        [HttpGet("s/GetAllAcceptProblem/{userId}")]
        public async Task<ActionResult<List<ProblemList>>> GetAllAcceptProblemsAsync(Guid userId)
        {
            var subjectDetails = await _teacherService.GetAllProblemsAsync(userId);

            if (subjectDetails.Count == 0)
                return Ok(subjectDetails);

            return Ok(subjectDetails);
        }

        [HttpPost("s/UpdateProblemFlag/{userId}/{postId}")]
        public async Task<ActionResult<List<ProblemPostAll>>> UpdateProblemFlag(Guid userId,Guid postId)
        {
            try
            {
                // Call the service method
                var updateDetails = await _teacherService.UpdateProblemFlagAsync(userId, postId);

                if (updateDetails == null || !updateDetails.Any())
                {
                    return NotFound("No records found after updating.");
                }

                return Ok(updateDetails);
            }
            catch (Exception ex)
            {
                // Log the exception if necessary
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
