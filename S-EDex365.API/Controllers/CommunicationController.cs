using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using S_EDex365.API.Interfaces;
using S_EDex365.API.Models;
using S_EDex365.API.Services;

namespace S_EDex365.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommunicationController : ControllerBase
    {
        private readonly ICommunicationService _communicationService;
        public CommunicationController(ICommunicationService communicationService)
        {
            _communicationService = communicationService;
        }
        [Route("s/SaveMessage")]
        [HttpPost]
        public async Task<ActionResult> SaveMessage(CommunicationDto communication, Guid userId, Guid problempostId)
        {
            await _communicationService.InsertChatAsync(communication, userId, problempostId);

            return Ok(new { message = "Successfully Submit" });
        }
    }
}
