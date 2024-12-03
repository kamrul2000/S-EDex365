using Microsoft.AspNetCore.Mvc;
using S_EDex365.Data.Interfaces;
using S_EDex365.Data.Services;
using S_EDex365.Model.Model;

namespace S_EDex365.Controllers
{
    public class SaveOtpController : Controller
    {
        //private readonly IUserService _userService;
        private readonly IOtpService _otpService;
        public SaveOtpController(IOtpService otpService)
        {
            _otpService = otpService ??
                throw new ArgumentNullException(nameof(otpService)); ;
        }
        [HttpGet]
        public IActionResult Otp(string userId, string mobileNo)
        {
            ViewBag.UserId = userId;
            return View();
        }
        //public async Task<IActionResult> Otp(User user)
        //{
        //    var success = await _otpService.UpdateOtpAsync(user);
        //    return Json(new { result = success });
        //}
        [HttpPost]
        public async Task<IActionResult> Otp(Guid userId, string otp)
        {
            var user = new User
            {
                Id = userId,
                OTP = otp
            };

            var success = await _otpService.UpdateOtpAsync(user);
            return Json(new { result = success });
        }
    }
}
