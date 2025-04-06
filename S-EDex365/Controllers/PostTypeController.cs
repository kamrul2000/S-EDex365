using Microsoft.AspNetCore.Mvc;
using S_EDex365.Data.Interfaces;
using S_EDex365.Model.Model;

namespace S_EDex365.Controllers
{
    public class PostTypeController : Controller
    {
        private readonly IPostTypeService _postTypeService;
        public PostTypeController(IPostTypeService service)
        {
            _postTypeService = service ??
                throw new ArgumentNullException(nameof(service));
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> GetAllPostType()
        {
            var postTypeList = await _postTypeService.GetAllPostTypeAsync();
            return Json(new { Data = postTypeList });
        }
        public async Task<IActionResult> AddPostType()
        {
            return PartialView("_AddPostType");
        }

        [HttpPost]
        public async Task<IActionResult> InsertPostType(PostType model)
        {
            try
            {
                var success = await _postTypeService.InsertPostTypeAsync(model);
                return Json(new { result = success });
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
