using Microsoft.AspNetCore.Mvc;
using S_EDex365.Data.Interfaces;
using S_EDex365.Model.Model;

namespace S_EDex365.Controllers
{
    public class ContactMeController : Controller
    {
        private readonly IContactMeService _contactMeService;
        public ContactMeController(IContactMeService contactMeService)
        {
            _contactMeService = contactMeService ??
                throw new ArgumentNullException(nameof(contactMeService));
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> GetAllContact()
        {
            var contactList = await _contactMeService.GetAllContactMeAsync();
            return Json(new { Data = contactList });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InsertContactMe(ContactMe model)
        {
            try
            {
                var success = await _contactMeService.InsertContactMeAsync(model);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
