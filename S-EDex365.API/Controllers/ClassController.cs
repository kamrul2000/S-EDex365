using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using S_EDex365.API.Interfaces;
using S_EDex365.API.Models;
using S_EDex365.API.Services;

namespace S_EDex365.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassController : ControllerBase
    {
        private readonly IClassService _classService;
        public ClassController(IClassService classService)
        {
            _classService = classService;
        }
        [Route("s/SaveClass")]
        [HttpPost]
        public async Task<ActionResult> SaveClass(ClassResponse classResponse)
        {
            var saveClassDetails = await _classService.InsertClassAsync(classResponse);
            var result = new
            {
                Message = "Successfully Submit",
                saveClassDetails = saveClassDetails
            };
            return Ok(result);
        }
        [HttpGet("s/AllClass")]
        public async Task<ActionResult<List<ClassResponse>>> GetAllClass()
        {
            var classDetails = await _classService.GetAllClassAsync();
            if (classDetails.Count == 0)
                return NotFound();
            return Ok(classDetails);
        }
    }
}
