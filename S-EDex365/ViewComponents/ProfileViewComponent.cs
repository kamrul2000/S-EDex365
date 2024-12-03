using Microsoft.AspNetCore.Mvc;
using S_EDex365.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace S_EDex365.Data.ViewComponents
{
    [ViewComponent(Name = "Profile")]
    public class ProfileViewComponent : ViewComponent
    {
        private readonly IUserService _userService;
        public ProfileViewComponent(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userService.GetUserAsync(Guid.Parse(userId));
            return View("Profile", user);
        }
    }
}
