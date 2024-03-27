using Microsoft.AspNetCore.Identity;
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
        private readonly SignInManager<AppUser> _signInManager;

        public LiteratureController(ILiteratureRepository literatureRepository, IHttpContextAccessor httpContextAccessor, SignInManager<AppUser> signInManager)
        {
            _literatureRepository = literatureRepository;
            _httpContextAccessor = httpContextAccessor;
            _signInManager = signInManager;
        }

        //      CREATE
        public IActionResult Create()
        {
            //  handle unauthenticated user
            if (!_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Unauthenticated", "Account");
            }
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

        //      READ
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

        //  read user records
        public async Task<IActionResult> Dashboard()
        {
            var userLiteratures = await _literatureRepository.GetAllUserLiteratures();
            var literatureDashboardVM = new LiteratureDashboardViewModel
            {
                Literatures = userLiteratures
            };

            return View(literatureDashboardVM);
        }

        //      UPDATE
        public async Task<IActionResult> Edit(int id)
        {
            //  fetch record details
            var literature = await _literatureRepository.GetByIdAsync(id);
            if (literature == null)
            {
                return View("Error");
            }
            //  pass data to viewmodel
            var editLiteratureVM = new EditLiteratureViewModel
            {
                Title = literature.Title,
                Description = literature.Description,
                LinkUrl = literature.LinkUrl,
                LiteratureCategory = literature.LiteratureCategory,
                LiteratureState = literature.LiteratureState,
            };
            return View(editLiteratureVM);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditLiteratureViewModel editLiteratureVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit lit");
                return View(editLiteratureVM);
            }
            var userLiterature = await _literatureRepository.GetByIdAsyncNoTracking(id);
            if (userLiterature != null)
            {
                var literature = new Literature
                {
                    Id = id,
                    Title = editLiteratureVM.Title,
                    Description = editLiteratureVM.Description,
                    LinkUrl = editLiteratureVM.LinkUrl,
                    LiteratureCategory = editLiteratureVM.LiteratureCategory,
                    LiteratureState = editLiteratureVM.LiteratureState,
                    AppUserId = userLiterature.AppUserId
                };
                _literatureRepository.Update(literature);
                return RedirectToAction("Index");
            }
            //  handle exceptions
            else
            {
                return View(editLiteratureVM);
            }
        }

        //      DELETE
        public async Task<IActionResult> Delete(int id)
        {
            var literatureDetails = await _literatureRepository.GetByIdAsync(id);
            //  record does not exist
            if (literatureDetails == null)
            {
                return View("Error");
            }
            //return the record for further action
            return View(literatureDetails);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteLiterature(int id)
        {
            var literatureDetails = await _literatureRepository.GetByIdAsync(id);
            if (literatureDetails == null)
            {
                //  handle missing record error
                return View("Error");
            }
            _literatureRepository.Delete(literatureDetails);
            return RedirectToAction("Dashboard");
        }
    }
}

