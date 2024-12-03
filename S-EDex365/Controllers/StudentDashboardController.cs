using Microsoft.AspNetCore.Mvc;
using S_EDex365.Authorization;
using S_EDex365.Data.Interfaces;
using S_EDex365.Model.Model;

namespace S_EDex365.Controllers
{
    public class StudentDashboardController : Controller
    {
        private readonly IStudentDashboardService _studentDashboardService;
        public StudentDashboardController(IStudentDashboardService studentDashboardService)
        {
            _studentDashboardService = studentDashboardService ??
                throw new ArgumentNullException(nameof(studentDashboardService));
        }

        //public async Task<IActionResult> Index()
        //{
        //    // Retrieve the username of the logged-in user
        //    //string username = User.Identity.IsAuthenticated ? User.Identity.Name : "Guest";

        //    // Pass the username to the view
        //    //ViewData["Username"] = username;


        //    // Retrieve the userId of the logged-in user
        //    string userId = User.Identity.IsAuthenticated ? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value : "Guest";
        //    var PendingProblem = await _studentDashboardService.GetAllPendingProblemAsync(userId);
        //    // Pass the userId to the view
        //    //ViewData["UserId"] = userId;

        //    return View();
        //}

        public async Task<IActionResult> Index()
        {
            string userId = User.Identity.IsAuthenticated
                ? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                : "Guest";

            // Ensure userId is not null
            if (userId == null)
            {
                userId = "Guest"; // or handle accordingly
            }

            //var pendingProblems = await _studentDashboardService.GetAllPendingProblemAsync(userId);

            // Pass the pending problems to the view
            StudentDashboard model = new StudentDashboard();

            var TotalPending = await _studentDashboardService.GetAllPendingProblemAsync(userId);
            model.PendingProblem = TotalPending.Count();
            var TotalProblems = await _studentDashboardService.GetAllPendingProblemAsync(userId);
            model.TotalProblem = TotalProblems.Count();
            var Solutions = await _studentDashboardService.GetAllPendingProblemAsync(userId);
            model.Solution = Solutions.Count();
            return View(model);
        }


    }
}
