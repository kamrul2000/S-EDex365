using Microsoft.AspNetCore.Mvc;
using S_EDex365.API.Interfaces;
using S_EDex365.API.Models;
using S_EDex365.Data.Interfaces;

namespace S_EDex365.API.Controllers
{
    public class StudentDashBoardController : Controller
    {
        private readonly IStudentDashBoard _studentDashBoard;
        public StudentDashBoardController(IStudentDashBoard studentDashBoard)
        {
            _studentDashBoard = studentDashBoard ??
                throw new ArgumentNullException(nameof(studentDashBoard));
        }

        [HttpGet("s/GetToatlService/{userId}")]
        public async Task<IActionResult> GetToatlService(Guid userId)
        {
            StudentDashBoard model = new StudentDashBoard();

            // Fetch and store the correct count
            var totalProblem = await _studentDashBoard.GetAllTotalProblemAsync(userId);
            model.TotalProblem = totalProblem?.Count() ?? 0;

            var totalPending = await _studentDashBoard.GetAllPendingProblemAsync(userId);
            model.PendingProblem = totalPending?.Count() ?? 0;

            var totalSolution = await _studentDashBoard.GetAllSolutionAsync(userId);
            model.Solution = totalSolution?.Count() ?? 0;

           

            return Ok(model);
        }
    }
}
