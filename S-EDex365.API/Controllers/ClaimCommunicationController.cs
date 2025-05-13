using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using S_EDex365.API.Interfaces;
using S_EDex365.API.Models;
using S_EDex365.API.Services;

namespace S_EDex365.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClaimCommunicationController : ControllerBase
    {
        private readonly IClaimCommunicationService _claimCommunicationService;
        public ClaimCommunicationController(IClaimCommunicationService claimCommunicationService)
        {
            _claimCommunicationService = claimCommunicationService;
        }
        [Route("s/ClaimSaveMessage")]
        [HttpPost]
        public async Task<ActionResult> ClaimSaveMessage([FromForm] ClaimCommunicationDto communication, [FromQuery] Guid userId, [FromQuery] Guid solutionId)
        {
            await _claimCommunicationService.InsertClaimCommunicationAsync(communication, userId, solutionId);
            return Ok(new { message = "Successfully Submit" });
        }
        [HttpGet("GetSolutionChat")]
        public async Task<ActionResult<List<SolutionChatResponse>>> GetSolutionChat([FromQuery] Guid solutionId)
        {
            if (solutionId == Guid.Empty)
            {
                return BadRequest("Invalid solutionId.");
            }

            var result = await _claimCommunicationService.GetSolutionChatAsync(solutionId);

            if (result == null || result.Count == 0)
            {
                return Ok("[]");
            }

            return Ok(result);
        }

    }
}
