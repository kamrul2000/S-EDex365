using Microsoft.AspNetCore.Mvc;
using S_EDex365.Data.Interfaces;
using S_EDex365.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace S_EDex365.Data.ViewComponents
{
    [ViewComponent(Name = "Menu")]
    public class MenuViewComponent : ViewComponent
    {
        private readonly IRoleService _roleService;
        private readonly IUserService _userService;
        public MenuViewComponent(IRoleService roleService, IUserService userService)
        {
            _roleService = roleService;
            _userService = userService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRole = await _userService.GetUserRolesAsync(userId);
            List<Menu> menuList = new List<Menu>();

            var roleName = new List<string>();
            foreach (var item in userRole)
            {
                Guid result = Guid.Parse(item);
                var name = await _roleService.GetRoleByIdAsync(result);
                roleName.Add(name.Name);
            }

            foreach (var item in userRole)
            {

                var permitedMenu = await _roleService.GetAllMenusAsync(Guid.Parse(item));
                if (roleName.Contains("Super Admin"))
                {
                    menuList.AddRange(permitedMenu);
                    break;
                }
                foreach (var menu in permitedMenu)
                {
                    if (menu.Status == true)
                    {
                        menuList.Add(menu);
                    }
                }
            }
            return View("Menu", menuList);
        }
    }
}
