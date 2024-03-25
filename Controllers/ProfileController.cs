using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShuffleLit.Interfaces;
using ShuffleLit.Models;
using ShuffleLit.ViewModels;

namespace ShuffleLit.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AppUser> _userManager;
        private readonly IProfileRepository _profileRepository;

        public ProfileController(IProfileRepository profileRepository, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _profileRepository = profileRepository;
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}

        public async Task<IActionResult> Index()
        {
            AppUser user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var userDetailVM = new UserDetailViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    PasswordChangedDate = user.PasswordChangedDate
                };
                return View(userDetailVM);
            }
            return View("Error");

        }
        //[HttpGet]
        //public async Task<IActionResult> Index(UserDetailViewModel userDetailVM)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        ModelState.AddModelError("", "Failed to load profile");
        //        return View("Index", userDetailVM);
        //    }
        //    AppUser user = await _profileRepository.GetByIdNoTracking(userDetailVM.Id);
        //    return View(userDetailVM);
        //}


        public IActionResult EditProfile()
        {
            return View();
        }
    }
}
