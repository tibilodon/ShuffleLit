using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShuffleLit.Models;
using ShuffleLit.ViewModels;

//  using IUserRepository
namespace ShuffleLit.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public ProfileController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

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
        public IActionResult EditProfile()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> EditProfile(EditProfileViewModel editProfileVM)
        {
            //  get current user
            AppUser user = await _userManager.GetUserAsync(User);
            //check if user is still logged in
            if (user != null)
            {
                //  generate token
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                //  append updated_at
                user.PasswordChangedDate = DateTime.Now;
                //  update password
                var result = await _userManager.ResetPasswordAsync(user, code, editProfileVM.Password);
                //  on success
                if (result.Succeeded)
                {
                    return RedirectToAction("ResetPasswordConfirmation", "Account");
                }
            }// handle error
            return View("Error");
        }
    }
}
