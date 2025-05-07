using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using S_EDex365.API.Interfaces;
using S_EDex365.API.Models;

namespace S_EDex365.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnglishClassController : ControllerBase
    {
        private readonly IEnglishClassService _classService;
        public EnglishClassController(IEnglishClassService classService)
        {
            _classService = classService;
        }
        [HttpGet("s/GetAllEnglishClass")]
        public async Task<ActionResult<List<EnglishClassResponse>>> GetAllEnglishClass()
        {
            var classDetails = await _classService.GetAllEnglishClassAsync();
            if (classDetails.Count == 0)
                return NotFound();
            return Ok(classDetails);
        }
    }
}
