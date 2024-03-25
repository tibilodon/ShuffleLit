using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShuffleLit.Interfaces;
using ShuffleLit.Models;
using ShuffleLit.ViewModels;

namespace ShuffleLit.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<AppUser> _userManager;

        public UserController(IUserRepository userRepository, UserManager<AppUser> userManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
        }

        //  get all users
        [HttpGet("users")]
        public async Task<IActionResult> Index()
        {
            //  get all users
            var users = await _userRepository.GetAllUsers();
            //  convert to list
            List<UserViewModel> result = new List<UserViewModel>();
            //  loop through and return required data
            foreach (var user in users)
            {
                var userVM = new UserViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    ProfileImageUrl = user.ProfileImageUrl,
                    PasswordChangedDate = user.PasswordChangedDate,
                };
                result.Add(userVM);
            }
            //  return the view with the data
            return View(result);
        }

        //  user details (single record)
        public async Task<IActionResult> Detail(string id)
        {
            var user = await _userRepository.GetUserById(id);
            var userDetailVM = new UserDetailViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                PasswordChangedDate = user.PasswordChangedDate


            };
            return View(userDetailVM);
        }
    }
}
