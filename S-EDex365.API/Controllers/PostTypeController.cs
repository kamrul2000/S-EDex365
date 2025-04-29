using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using S_EDex365.API.Interfaces;
using S_EDex365.API.Models;
using S_EDex365.API.Services;

namespace S_EDex365.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostTypeController : ControllerBase
    {
        private readonly IPostTypeService _postTypeService;
        public PostTypeController(IPostTypeService postTypeService)
        {
            _postTypeService = postTypeService;
        }

        [HttpGet("s/GetAllPostType")]
        public async Task<ActionResult<List<PostType>>> GetAllPostType()
        {
            var postTypeDetails = await _postTypeService.GetAllPostTypeAsync();
            if (postTypeDetails.Count == 0)
                return NotFound();
            return Ok(postTypeDetails);
        }
    }
}
