using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using S_EDex365.Data.Interfaces;
using S_EDex365.Model.Model;

namespace S_EDex365.Controllers
{
    public class PostTypeDetailsController : Controller
    {
        private readonly IPostTypeDetailsService _postTypeDetailsService;
        public PostTypeDetailsController(IPostTypeDetailsService service)
        {
            _postTypeDetailsService = service ??
                throw new ArgumentNullException(nameof(service));
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> GetAllPostTypeDetails()
        {
            var postTypeDetailsList = await _postTypeDetailsService.GetAllPostTypeDetailsAsync();
            return Json(new { Data = postTypeDetailsList });
        }
        public async Task<IActionResult> AddPostTypeDetails()
        {
            var postTypedetailsList = await _postTypeDetailsService.GetAllPostTypeAsync();
            ViewBag.PostTypeDetailsList = postTypedetailsList.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            return PartialView("_AddPostTypeDetails");
        }

        [HttpPost]
        public async Task<IActionResult> InsertPostTypeDetails(PostTypeDetails model)
        {
            try
            {
                var success = await _postTypeDetailsService.InsertPostTypeDetailsAsync(model);
                return Json(new { result = success });
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public async Task<IActionResult> UpdatePostTypeDetails(Guid postTypeDetailsId)
        {
            var serviceAndPart = await _postTypeDetailsService.GetPostTypeDetailsByIdAsync(postTypeDetailsId);
            return PartialView("_UpdatePostType", serviceAndPart);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePostTypeDetailsPartail(PostTypeDetails model)
        {
            try
            {
                var success = await _postTypeDetailsService.UpdatePostTypeDetailsAsync(model);
                return Json(new { result = success });
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeletePostTypeDetails(Guid postTypeDetailsId)
        {
            try
            {
                var result = await _postTypeDetailsService.DeletePostTypeDetailsAsync(postTypeDetailsId);
                return Json(new { result });
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
