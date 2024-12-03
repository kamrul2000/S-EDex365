using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using S_EDex365.API.Interfaces;
using S_EDex365.API.Models;
using S_EDex365.API.Services;

namespace S_EDex365.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherApprovalController : ControllerBase
    {
        private readonly ITeacherApprovalService _service;

        public TeacherApprovalController(ITeacherApprovalService service)
        {
            _service = service;
        }

        [HttpPut("s/TeacherApproval")]
        public async Task<ActionResult<UserResponse>> TeacherApproval([FromBody] TeacherApprovalDto teacherApprovalDto)
        {
            var otpDetails = await _service.UpdateTeacherApprovalAsync(teacherApprovalDto);

            var result = new
            {
                Message = teacherApprovalDto.Aproval == true ? "Teacher Is Approved.." : "Data Has been deleted."
            };
            return Ok(result);
        }

        [HttpGet("s/GetAllPreApprovalTeacher")]
        public async Task<ActionResult<List<TeacherApprovalResponse>>> GetAllPreApprovalTeacher()
        {
            var teacherApprovalDetails = await _service.GetAllTeacherApprovalListAsync();
            if (teacherApprovalDetails.Count == 0)
                return NotFound();
            return Ok(teacherApprovalDetails);
        }

    }
}
