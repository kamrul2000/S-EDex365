using Microsoft.AspNetCore.Mvc;
using S_EDex365.Authorization;
using S_EDex365.Data.Interfaces;
using S_EDex365.Model.Model;

namespace S_EDex365.Controllers
{
    public class AdminDashboardController : Controller
    {
        private readonly IAdminDashboardService _admindashboard;
        public AdminDashboardController(IAdminDashboardService admindashboard)
        {
            _admindashboard = admindashboard ??
                throw new ArgumentNullException(nameof(admindashboard));
        }

        public async Task<IActionResult> Index()
        {
            AdminDashboard model= new AdminDashboard();
            
            var TotalProblems = await _admindashboard.GetAllTotalProblemAsync();
            model.TotalProblems = TotalProblems;

            var PendingProblem = await _admindashboard.GetAllPendingAsync();
            model.PendingSolutions = PendingProblem;

            var TotalSolution=await _admindashboard.GetAllSolutionAsync();
            model.TotalSolutions = TotalSolution;

            return View(model);
        }
    }
}
