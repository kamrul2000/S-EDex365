using Microsoft.AspNetCore.Mvc;
using S_EDex365.Data.Interfaces;

namespace S_EDex365.Controllers
{
    public class ProblemsPostController : Controller
    {
        private readonly IProblemsPost _problemsPost;
        public ProblemsPostController(IProblemsPost problemsPost)
        {
            _problemsPost = problemsPost ??
                throw new ArgumentNullException(nameof(problemsPost)); ;
        }
        [HttpGet]
        public IActionResult ProblemsPost()
        {
            return View();
        }
    }
}
