using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using S_EDex365.API.Interfaces;
using S_EDex365.API.Models;
using S_EDex365.API.Services;
using S_EDex365.Model.Model;

namespace S_EDex365.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OtpController : ControllerBase
    {
        private readonly IOtpService _service;
        public OtpController(IOtpService service)
        {
            _service = service;
        }
        
        [HttpPost("s/VerifyOtp")]
        public async Task<ActionResult<UserResponse>> VerifyOtp([FromBody] OtpDto otpDto)
        {
            var otpDetails = await _service.UpdateOtpAsync(otpDto);
            if (otpDetails != null)
            {
                return Ok("Verifyed Otp..");
            }
            else
            {
                return Ok("Otp not Valid.");
            }
        }
    }
}
