using Microsoft.AspNetCore.Mvc;

namespace S_EDex365.Controllers
{
	public class AdminController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
