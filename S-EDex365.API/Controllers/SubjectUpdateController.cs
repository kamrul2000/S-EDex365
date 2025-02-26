using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using S_EDex365.API.Interfaces;
using S_EDex365.API.Models;
using S_EDex365.API.Services;

namespace S_EDex365.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectUpdateController : ControllerBase
    {
        private readonly ISubjectUpdateService _subjectUpdateService;
        public SubjectUpdateController(ISubjectUpdateService subjectUpdateService)
        {
            _subjectUpdateService = subjectUpdateService;
        }


        [Route("s/TeacherSkill")]
        [HttpPut]
        public async Task<ActionResult<SubjectResponseUpdate>> TeacherSkills([FromForm] SubjectDtoUpdate subjectDtoUpdate)
        {
            // Call the service method to update the subjects
            var updateSkill = await _subjectUpdateService.UpdateSubjectAsync(subjectDtoUpdate);

            // Return a success message along with the updated details
            var result = new
            {
                Message = "Successfully Added",
                teacherUpdateSkill = updateSkill
            };

            return Ok(result);
        }

        [HttpGet("s/AllSkill/{userId}")]
        public async Task<ActionResult<List<SubjectResponseUpdate>>> GetAllSkill(Guid userId)
        {
            var result = await _subjectUpdateService.GetAllSubjectResponseUpdateAsync(userId);
            if (result.Count == 0)
                return Ok("No problem posts found for the specified userss");
            return Ok(result);
        }

        [HttpPost("s/DeleteSkill")]
        public async Task<IActionResult> DeleteSkill(Guid userId, [FromBody] List<Guid> subIds)
        {
            if (subIds == null || subIds.Count == 0)
            {
                return BadRequest("No subjects provided for deletion.");
            }
            var result = await _subjectUpdateService.DeleteUserAsync(userId, subIds);

            if (!result)
            {
                return NotFound("Subjects could not be deleted.");
            }

            return Ok("skill are deleted...");
        }

        //[HttpPost("s/DeleteSkill")]
        //public async Task<IActionResult> DeleteSkill(Guid userId, Guid subId)
        //{
        //    var result = await _subjectUpdateService.DeleteUserAsync(userId,subId);
        //    if (result == null)
        //        return NotFound("Subject are not Delete...");
        //    return Ok("Skill are Deleted...");
        //}


    }
}
