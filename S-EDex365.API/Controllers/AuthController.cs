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
        //[Route("s/Signup")]
        //[HttpPost]
        ////public async Task<ActionResult<UserResponse>> Signup([FromBody] UserDto userDto)
        //    public async Task<ActionResult<UserResponse>> Signup([FromForm] UserDto userDto)
        //{
        //    var signupDetails=await _authService.InsertUserAsync(userDto);
        //    var result = new
        //    {
        //        Message = "Successfully Submit",
        //        signupDetails= signupDetails
        //    };
        //    return Ok(result);
        //}

        [Route("s/Signup")]
        [HttpPost]
        public async Task<ActionResult<UserResponse>> Signup([FromForm] UserDto userDto)
        {
            if (userDto == null)
            {
                return BadRequest(new { Message = "Invalid request data" });
            }

            try
            {
                var signupDetails = await _authService.InsertUserAsync(userDto);
                if (signupDetails == null)
                {
                    return BadRequest(new { Message = "Signup failed. User might already exist." });
                }

                var result = new
                {
                    Message = "Successfully Submitted",
                    SignupDetails = signupDetails
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred", Error = ex.Message });
            }
        }


        [Route("s/Update")]
        [HttpPut]
        public async Task<ActionResult<UserResponseUpdate>> Update([FromForm] UserDtoUpdate userDtoUpdate)
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

        [HttpPut("s/UpdatePassword")]
        public async Task<ActionResult> UpdatePassword(Guid userId,string oldPassWord,string newPassWord)
        {
            var result = await _authService.UpdatePasswordserAsync(userId,oldPassWord,newPassWord);
            if (!result) // If false, return NotFound
                return NotFound("Password isn't updated. Old password is incorrect.");
            return Ok("PassWord is Update....");
        }
        [HttpPut("s/ForgotPasswordSendOTP")]
        public async Task<ActionResult> ForgotPasswordSendOTP(ForgotPasswordSendOTP forgotPasswordSendOTP)
        {
            var result = await _authService.ForgotPasswordSendOTPAsync(forgotPasswordSendOTP);
            if (!result)
                return NotFound(new { message = "Phone Number is not Correct..." });

            return Ok(new { message = "Success" });
        }
        [HttpPut("s/ForgotPasswordVerifyOTP")]
        public async Task<ActionResult> ForgotPasswordVerifyOTP(ForgotPasswordUpdateOTP forgotPasswordUpdateOTP)
        {
            var result = await _authService.ForgotPasswordVerifyOTPAsync(forgotPasswordUpdateOTP);
            if (!result)
                return NotFound(new { message = "OTP is not Correct..." });

            return Ok(new { message = "Success" });
        }
        [HttpPut("s/ForgotPasswordConfirm")]
        public async Task<ActionResult> ForgotPasswordConfirm(ForgotPasswordConfirm forgotPasswordConfirm)
        {
            var result = await _authService.ForgotPasswordConfirmAsync(forgotPasswordConfirm);
            if (!result)
                return NotFound(new { message = "Faild..." });

            return Ok(new { message = "Success" });
        }

        [HttpGet("GetServerDateTime")]
        public async Task<ActionResult<string>> GetServerDateTime()
        {
            return DateTime.Now.ToString();
        }
    }
}
