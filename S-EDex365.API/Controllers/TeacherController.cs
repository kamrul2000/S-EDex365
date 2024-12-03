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
        public async Task<ActionResult<List<ProblemPostAll>>> GetProblems(Guid userId)
        {
            var subjectDetails = await _teacherService.GetAllPostByUserAsync(userId);

            if (subjectDetails.Count == 0)
                return NotFound();

            return Ok(subjectDetails);
        }
        [HttpPut("s/UpdateProblemFlag/{postId}")]
        public async Task<ActionResult<List<ProblemPostAll>>> UpdateProblemFlag(Guid postId)
        {
            var updateDetails = await _teacherService.UpdateProblemFlagAsync(postId);
            return Ok("This Task Accepted..");
        }
    }
}
