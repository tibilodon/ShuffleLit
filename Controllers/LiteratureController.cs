using Microsoft.AspNetCore.Mvc;
using ShuffleLit.Interfaces;

namespace ShuffleLit.Controllers
{
    public class LiteratureController : Controller
    {
        public LiteratureController(ILiteratureRepository literatureRepository, IHttpContextAccessor httpContextAccessor) { }
        public IActionResult Index()
        {
            return View();
        }
    }
}
