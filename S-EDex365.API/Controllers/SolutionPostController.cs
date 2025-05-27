using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using S_EDex365.API.Interfaces;
using S_EDex365.API.Models;
using S_EDex365.API.Services;

namespace S_EDex365.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolutionPostController : ControllerBase
    {
        private readonly ISolutionPostService _solutionPostService;
        public SolutionPostController(ISolutionPostService solutionPostService)
        {
            _solutionPostService = solutionPostService;
        }

        [HttpGet("s/Solution/{postId}")]
        public async Task<ActionResult<List<SolutionShowAll>>> GetProblems(Guid postId)
        {
            var subjectDetails = await _solutionPostService.GetAllSolutionAsync(postId);
            if (subjectDetails.Count == 0)
                return Ok();
            return Ok(subjectDetails);
        }

        //[HttpPost("s/SolutionPost/{postId}")]
        //public async Task<IActionResult> UploadSolution([FromForm] SolutionPostDto solutionPost,Guid  postId)
        //{
        //    if (postId == Guid.Empty)
        //    {
        //        return BadRequest("Invalid postId.");
        //    }

        //    // Validate TeacherId and Photo
        //    if (solutionPost.TeacherId == Guid.Empty)
        //    {
        //        return BadRequest("TeacherId is required.");
        //    }

        //    if (solutionPost.Photo == null || solutionPost.Photo.Length == 0)
        //    {
        //        return BadRequest("Photo is required.");
        //    }

        //    // Call the service method
        //    var result = await _solutionPostService.InsertSolutionPostAsync(solutionPost, postId);

        //    if (result == null)
        //    {
        //        return StatusCode(500, "Internal Server Error: Failed to save solution post.");
        //    }

        //    return Ok(result);
        //}


        [HttpPost("s/SolutionPost/{postId}")]
        public async Task<IActionResult> UploadSolution([FromForm] SolutionPostDto solutionPost, Guid postId)
        {
            if (postId == Guid.Empty)
            {
                return BadRequest("Invalid postId.");
            }

            // Validate TeacherId and Photos
            if (solutionPost.TeacherId == Guid.Empty)
            {
                return BadRequest("TeacherId is required.");
            }

            if (solutionPost.Photos == null || !solutionPost.Photos.Any())
            {
                return BadRequest("At least one photo is required.");
            }

            try
            {
                // Call the service method
                var result = await _solutionPostService.InsertSolutionPostAsync(solutionPost, postId);

                if (result == null || !result.Any())
                {
                    return StatusCode(500, "Internal Server Error: Failed to save solution post.");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }


    }
}
