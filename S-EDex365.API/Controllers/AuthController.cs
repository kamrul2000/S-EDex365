using Microsoft.AspNetCore.Mvc;
using S_EDex365.API.Interfaces;
using S_EDex365.API.Models;
using S_EDex365.Model.Model;

namespace S_EDex365.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        
        [Route("Login")]
        [HttpPost]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginDto login)
        {
            var loginDetails = await _authService.Login(login);
            if ( !string.IsNullOrEmpty( loginDetails.Id.ToString()))
            {
                return loginDetails;
            }
            return loginDetails;

            return BadRequest(new { Ok = false, Message = "Username and Password does not march" });
        }
        [Route("s/Signup")]
        [HttpPost]
        public async Task<ActionResult<UserResponse>> Signup([FromBody] UserDto userDto)
        {
            var signupDetails=await _authService.InsertUserAsync(userDto);
            var result = new
            {
                Message = "Successfully Submit",
                signupDetails= signupDetails
            };
            return Ok(result);
        }

        [Route("s/Update")]
        [HttpPut]
        public async Task<ActionResult<UserResponseUpdate>> Signup([FromForm] UserDtoUpdate userDtoUpdate)
        {
            var updateDetails = await _authService.UpdateUserAsync(userDtoUpdate);
            var result = new
            {
                Message = "Successfully Updated",
                updateDetails = updateDetails
            };
            return Ok(result);
        }

        [HttpGet("s/User/{userId}")]
        public async Task<ActionResult<List<UserAllInformation>>> GetAllUser(Guid userId)
        {
            var result = await _authService.GetAllUserInformationAsync(userId);
            if (result.Count == 0)
                return NotFound("No problem posts found for the specified user");
            return Ok(result);
        }

    }
}
