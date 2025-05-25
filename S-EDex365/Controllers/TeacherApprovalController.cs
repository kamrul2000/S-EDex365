using Microsoft.AspNetCore.Mvc;
using S_EDex365.Data.Interfaces;
using S_EDex365.Data.Services;
using S_EDex365.Model.Model;

namespace S_EDex365.Controllers
{
    public class TeacherApprovalController : Controller
    {
        private readonly ITeacherApprovalService _teacherApproval;
        public TeacherApprovalController(ITeacherApprovalService teacherApproval)
        {
            _teacherApproval = teacherApproval ??
                throw new ArgumentNullException(nameof(teacherApproval));
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> GetAllTeacherApproval()
        {
            var teacherApprovalList = await _teacherApproval.GetAllTeacherApprovalListAsync();
            return Json(new { Data = teacherApprovalList });
        }
        public async Task<IActionResult> UpdateTeacherApproval(Guid userId)
        {
            var serviceAndPart = await _teacherApproval.GetTeacherApprovaByIdAsync(userId);

            if (serviceAndPart == null)
            {
                return NotFound();  // Handle null case (e.g., return a 404 response)
            }

            return PartialView("_TeacherApproved", serviceAndPart);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTeacherApprovalPartail([FromBody] TeacherApproval teacherApproval)
        {
            if (teacherApproval == null)
            {
                return BadRequest("Teacher approval data is null.");
            }

            try
            {
                var success = await _teacherApproval.UpdateTeacherApprovalAsync(teacherApproval);
                return Json(new { result = success });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
        public IActionResult GetApprovalAfterLoginIndex()
        {
            return View();
        }
        public async Task<IActionResult> GetAllTeacherApprovalAfterLogin()
        {
            var teacherApprovalList = await _teacherApproval.GetALLTeacherApprovalListAfterLoginbyAsync();
            return Json(new { Data = teacherApprovalList });
        }
        public async Task<IActionResult> UpdateTeacherApprovalAfterLogin(Guid Id)
        {
            var serviceAndPart = await _teacherApproval.GetTeacherApprovaAfterLoginByIdAsync(Id);

            if (serviceAndPart == null)
            {
                return NotFound();  // Handle null case (e.g., return a 404 response)
            }

            return PartialView("_TeacherApprovedAfterLogin", serviceAndPart);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateTeacherApprovalAfterLoginPartail([FromBody] TeacherApproval teacherApproval)
        {
            if (teacherApproval == null)
            {
                return BadRequest("Teacher approval data is null.");
            }

            try
            {
                var success = await _teacherApproval.UpdateTeacherApprovalAfterLoginAsync(teacherApproval);
                return Json(new { result = success });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }



    }
}
