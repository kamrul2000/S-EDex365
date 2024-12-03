using Microsoft.AspNetCore.Mvc;
using S_EDex365.Authorization;

namespace S_EDex365.Controllers
{
    public class TeacherDashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
