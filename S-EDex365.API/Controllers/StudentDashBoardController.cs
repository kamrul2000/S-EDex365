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
            model.TotalProblem = totalProblem;

            var totalPending = await _studentDashBoard.GetAllPendingProblemAsync(userId);
            model.PendingProblem = totalPending;

            var totalSolution = await _studentDashBoard.GetAllSolutionAsync(userId);
            model.Solution = totalSolution;

           

            return Ok(model);
        }
    }
}
