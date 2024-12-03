using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using S_EDex365.API.Interfaces;
using S_EDex365.API.Models;

namespace S_EDex365.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }
        [HttpGet("s/AllRoles")]
        public async Task<ActionResult<List<RoleResponse>>> GetAllRoles()
        {
            var roleDetails=await _roleService.GetRolesAsync();
            if(roleDetails.Count==0)
                return NotFound();
            return Ok(roleDetails);
        }
    }
}
