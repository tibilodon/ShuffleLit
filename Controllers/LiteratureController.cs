using Microsoft.AspNetCore.Mvc;
using ShuffleLit.Interfaces;
using ShuffleLit.Models;
using ShuffleLit.ViewModels;

namespace ShuffleLit.Controllers
{
    public class LiteratureController : Controller
    {
        private readonly ILiteratureRepository _literatureRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LiteratureController(ILiteratureRepository literatureRepository, IHttpContextAccessor httpContextAccessor)
        {
            _literatureRepository = literatureRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        //  return all
        public async Task<IActionResult> Index()
        {
            IEnumerable<Literature> literatures = await _literatureRepository.GetAll();
            return View(literatures);
        }
        //  detail one record
        public async Task<IActionResult> Detail(int id)
        {
            Literature literature = await _literatureRepository.GetByIdAsync(id);
            return View(literature);
        }
        //  create


        public IActionResult Create()
        {
            var curUserId = _httpContextAccessor.HttpContext.User.GetUserId();
            var createLiteratureVM = new CreateLiteratureViewModel
            {
                AppUserId = curUserId,
            };
            return View(createLiteratureVM);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateLiteratureViewModel createLiteratureVM)
        {
            if (ModelState.IsValid)
            {

                var literature = new Literature
                {
                    Title = createLiteratureVM.Title,
                    Description = createLiteratureVM.Description,
                    AppUserId = createLiteratureVM.AppUserId,
                    LinkUrl = createLiteratureVM.LinkUrl,
                    LiteratureCategory = createLiteratureVM.LiteratureCategory,
                    LiteratureState = createLiteratureVM.LiteratureState


                };
                _literatureRepository.Add(literature);
                return RedirectToAction("Index");
            }
            //  handle modelstate error
            else
            {
                ModelState.AddModelError("", "Could not create new record");
            }
            //  handle other exceptions
            return View(createLiteratureVM);
        }
        //  user records
        public async Task<IActionResult> Dashboard()
        {
            var userLiteratures = await _literatureRepository.GetAllUserLiteratures();
            var literatureDashboardVM = new LiteratureDashboardViewModel
            {
                Literatures = userLiteratures
            };

            return View(literatureDashboardVM);
        }
    }
}

