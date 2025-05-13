using Microsoft.AspNetCore.Mvc;
using S_EDex365.Data.Interfaces;
using S_EDex365.Data.Services;

namespace S_EDex365.Controllers
{
    public class ClaimTaskController : Controller
    {
        private readonly IClaimTaskService _claimTaskService;
        public ClaimTaskController(IClaimTaskService claimTaskService)
        {
            _claimTaskService = claimTaskService ??
                throw new ArgumentNullException(nameof(claimTaskService));
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetAllContact()
        {
            var contactList = await _claimTaskService.GetAllClaimbyAsync();
            return Json(new { Data = contactList });
        }
    }
}
