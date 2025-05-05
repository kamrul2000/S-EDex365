using Microsoft.AspNetCore.Mvc;
using S_EDex365.Data.Interfaces;
using S_EDex365.Data.Services;
using S_EDex365.Model.Model;

namespace S_EDex365.Controllers
{
    public class BanglaClassController : Controller
    {
        private readonly IBanglaClassService _banglaClass;
        public BanglaClassController(IBanglaClassService service)
        {
            _banglaClass = service ??
                throw new ArgumentNullException(nameof(service));
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> GetAllBanglaClass()
        {
            var banglaClassList = await _banglaClass.GetAllBanglaClassAsync();
            return Json(new { Data = banglaClassList });
        }
        public async Task<IActionResult> AddBanglaClass()
        {
            return PartialView("_AddBanglaClass");
        }

        [HttpPost]
        public async Task<IActionResult> InsertBanglaClass(BanglaClass model)
        {
            try
            {
                var success = await _banglaClass.InsertBanglaClassAsync(model);
                return Json(new { result = success });
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<IActionResult> UpdateBanglaCLass(Guid classId)
        {
            var serviceAndPart = await _banglaClass.GetBanglaClassByIdAsync(classId);
            return PartialView("_UpdateBanglaClass", serviceAndPart);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateBanglaClassPartail(BanglaClass model)
        {
            try
            {
                var success = await _banglaClass.UpdateBanglaClassAsync(model);
                return Json(new { result = success });
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteBanglaClass(Guid classId)
        {
            try
            {
                var result = await _banglaClass.DeleteBanglaClassAsync(classId);
                return Json(new { result });
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
