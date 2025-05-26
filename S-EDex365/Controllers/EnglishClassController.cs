using Microsoft.AspNetCore.Mvc;
using S_EDex365.Data.Interfaces;
using S_EDex365.Model.Model;

namespace S_EDex365.Controllers
{
    public class EnglishClassController : Controller
    {
        private readonly IEnglishClassService _englishClassService;
        public EnglishClassController(IEnglishClassService englishClassService)
        {
            _englishClassService = englishClassService ??
                throw new ArgumentNullException(nameof(englishClassService));
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> GetAllEnglishClass()
        {
            var banglaClassList = await _englishClassService.GetAllEnglishClassAsync();
            return Json(new { Data = banglaClassList });
        }
        public async Task<IActionResult> AddEnglishClass()
        {
            return PartialView("_AddEnglishClass");
        }

        [HttpPost]
        public async Task<IActionResult> InsertEnglishClass(EnglishClass model)
        {
            try
            {
                var success = await _englishClassService.InsertEnglishClassAsync(model);
                return Json(new { result = success });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IActionResult> UpdateEnglishCLass(Guid classId)
        {
            var serviceAndPart = await _englishClassService.GetEnglishClassByIdAsync(classId);
            return PartialView("_UpdateenglishClass", serviceAndPart);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateEnglishlassPartail(EnglishClass model)
        {
            try
            {
                var success = await _englishClassService.UpdateEnglishClassAsync(model);
                return Json(new { result = success });
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteEnglishClass(Guid classId)
        {
            try
            {
                var result = await _englishClassService.DeleteEnglishClassAsync(classId);
                return Json(new { result });
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
