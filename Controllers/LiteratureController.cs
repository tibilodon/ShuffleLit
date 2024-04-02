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
        private readonly IPlaceholderImgService _placeholderImg;
        private readonly ILiteratureCollectionRepository _literatureCollectionRepository;
        private readonly UserManager<AppUser> _userManager;

        public LiteratureController(ILiteratureRepository literatureRepository, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IPlaceholderImgService placeholderImg, ILiteratureCollectionRepository literatureCollectionRepository)
        {
            _literatureRepository = literatureRepository;
            _httpContextAccessor = httpContextAccessor;
            _signInManager = signInManager;
            _placeholderImg = placeholderImg;
            _literatureCollectionRepository = literatureCollectionRepository;
            _userManager = userManager;
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
                //  add record to collection
                var literatureCollection = new LiteratureCollection
                {
                    AppUserId = createLiteratureVM.AppUserId,
                    LiteratureId = literature.Id
                };
                _literatureCollectionRepository.Add(literatureCollection);


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
            var placeholderIcon = _placeholderImg.PlaceholderImg(literature.LiteratureCategory);
            ViewData["ImgUrl"] = placeholderIcon;
            return View(literature);
        }

        //  read user records
        public async Task<IActionResult> Dashboard()
        {
            var userLiteratures = await _literatureRepository.GetAllUserLiteratures();
            var literatureDashboardVM = new LiteratureDashboardViewModel
            {
                Literatures = userLiteratures,
            };

            return View(literatureDashboardVM);
        }

        //  read user collections
        public async Task<IActionResult> Collection()
        {
            var userCollections = await _literatureCollectionRepository.GetLiteratureCollectionForUser();
            var literatureCollectionVM = new LiteratureCollectionViewModel
            {
                Literatures = userCollections,
            };
            return View(literatureCollectionVM);
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
        //  delete literature with collection
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
            var curUser = _userManager.GetUserId(User);
            if (literatureDetails == null)
            {
                //  handle missing record error
                return View("Error");
            }
            if (literatureDetails.AppUserId == curUser)
            {

                //  author of the record, no need to handle join table deletes
                _literatureRepository.Delete(literatureDetails);
            }
            return RedirectToAction("Dashboard");
        }

        //  delete (only) literature collection - !author
        [HttpPost]
        public async Task<IActionResult> DeleteFromCollection(int id)
        {
            var curUser = _userManager.GetUserId(User);

            var literatureDetails = await _literatureRepository.GetByIdAsync(id);
            if (literatureDetails == null)
            {
                //  handle missing record error
                return View("Error");
            }
            //  async
            await _literatureCollectionRepository.DeleteLiteratureCollectionFromUser(curUser, id);
            return RedirectToAction("Index");
        }

        //  add literature to collection
        [HttpPost]
        public async Task<IActionResult> AddToCollection(int id)
        {
            //  get the logged in user
            AppUser user = await _userManager.GetUserAsync(User);
            //  fetch the record
            var literature = await _literatureRepository.GetByIdAsync(id);
            //  fetch the join table
            //  see if record is already in their collection
            var literatureCollection = await _literatureCollectionRepository.FindAppUserCollectionById(user.Id, id);
            //  if user does not have the join table
            if (literatureCollection == null)
            {
                //  save it to collection - AddLiteratureToUser
                _literatureCollectionRepository.AddLiteratureToUser(user.Id, literature.Id);
                //  redirect the user back to index page
                return RedirectToAction("Index");
            }
            //  handle error
            return View("Error");

        }

    }
}

