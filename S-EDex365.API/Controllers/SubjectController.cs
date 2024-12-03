using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using S_EDex365.API.Interfaces;
using S_EDex365.API.Models;
using S_EDex365.API.Services;

namespace S_EDex365.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectService _subjectService;
        public SubjectController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }
        [Route("SaveSubject")]
        [HttpPost]
        public async Task<ActionResult> SaveSubject(SubjectResponse subject)
        {
            var saveSubjectDetails= await _subjectService.InsertSubjectAsync(subject);
            var result = new
            {
                Message = "Successfully Submit",
                saveSubjectDetails = saveSubjectDetails
            };
            return Ok(result);
        }
        [HttpGet("s/AllSubject")]
        public async Task<ActionResult<List<SubjectResponse>>> GetAllClass()
        {
            var subjectDetails = await _subjectService.GetAllSubjectAsync();
            if (subjectDetails.Count == 0)
                return NotFound();
            return Ok(subjectDetails);
        }
    }
}
