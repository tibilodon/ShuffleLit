using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShuffleLit.Data;
using ShuffleLit.Interfaces;
using ShuffleLit.Models;
using ShuffleLit.ViewModels;

namespace ShuffleLit.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly IGSMTPService _gsmtpService;

        public AccountController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager, ApplicationDbContext context, IGSMTPService gsmtpService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _context = context;
            _gsmtpService = gsmtpService;
        }

        //  register

        public async Task<IActionResult> Register(string? returnUrl = null)
        {
            //  check for existimg role
            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
            {
                //  create "user" role
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));
            }
            RegisterViewModel registerVM = new RegisterViewModel();
            registerVM.ReturnUrl = returnUrl;
            //registerVM.PasswordChangedDate = DateTime.Now;

            return View(registerVM);
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerVM, string? returnUrl = null)
        {
            registerVM.ReturnUrl = returnUrl;
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                //  see if user already exists
                var user = await _userManager.FindByEmailAsync(registerVM.EmailAddress);
                //  user exists
                if (user != null)
                {
                    TempData["Error"] = "This email address is already in use";
                    return View(registerVM);
                }
                //  otherwise, create new user
                //  using () as AppUser Class inherits from IdentityUser
                var newUser = new AppUser()
                {
                    Email = registerVM.EmailAddress,
                    //  TODO: change this
                    UserName = registerVM.EmailAddress,
                    PasswordChangedDate = DateTime.Now,

                };
                var newUserResponse = await _userManager.CreateAsync(newUser, registerVM.Password);
                if (newUserResponse.Succeeded)
                {
                    //  add USER role to new user
                    await _userManager.AddToRoleAsync(newUser, UserRoles.User);

                    //  sign in new user
                    await _signInManager.SignInAsync(newUser, isPersistent: false);
                    //  redirect
                    return LocalRedirect(returnUrl);
                }
                //  password is not secure enough
                ModelState.AddModelError("Password", "User could not be created. Password is not unique enough");
            }
            return View(registerVM);
        }


        //  login
        public IActionResult Login()
        {
            //  accidental reload will hold typed in data
            var response = new LoginViewModel();
            return View(response);
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            //  handle errors from model binding and model validation
            if (!ModelState.IsValid)
            {
                return View(loginVM);
            }
            var user = await _userManager.FindByEmailAsync(loginVM.EmailAddress);
            //  check (if any) user's pw
            if (user != null)
            {
                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginVM.Password);
                //  password is valid
                if (passwordCheck)
                {
                    //  TODO: modify isPersistent, lockoutOnFailure
                    var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, true);
                    //  on success
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    // TODO: does not run - lock out users after n tries
                    if (result.IsLockedOut)
                    {
                        return View("Lockout");
                    }
                }
                //  incorrect pw
                TempData["Error"] = "Wrong credentials, Please try again - incorrect password";
                return View(loginVM);

            }
            //  handle error - user not found
            TempData["Error"] = "Could not find user";
            return View(loginVM);

        }
        //  logout
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        //  forgot password
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel forgotPasswordVM)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(forgotPasswordVM.Email);
                if (user == null)
                {
                    TempData["Error"] = "This email address does not exists";
                    return View(forgotPasswordVM);
                }
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                //  generate token
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                //  send out confirmation email with token
                await _gsmtpService.SendEmailAsync(forgotPasswordVM.Email, "Reset email confirmation", $"Please click on this link to reset your password: <a href=\"{callbackUrl}\">Reset Password<a/>");
                //  redirect
                return RedirectToAction("ForgotPasswordConfirmation");
            }
            return View(forgotPasswordVM);
        }

        //  forgot password email sent out - confirmation page
        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //  reset password
        [HttpGet]
        //  small project does not necessairly need async
        public IActionResult ResetPassword(string code = null)
        {
            return code == null ? View("Error") : View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordVM)
        {
            if (ModelState.IsValid)
            {
                //  find user by email address
                var user = await _userManager.FindByEmailAsync(resetPasswordVM.Email);
                user.PasswordChangedDate = DateTime.Now;
                //  email address is not present in DB
                if (user == null)
                {
                    //  provide error
                    ModelState.AddModelError("Email", "This email address has not been registered");
                    return View();
                }
                //  otherwise, update PW
                //resetPasswordVM.PasswordChangedDate = DateTime.Now;

                var result = await _userManager.ResetPasswordAsync(user, resetPasswordVM.Code, resetPasswordVM.Password);
                //  on success
                if (result.Succeeded)
                {
                    return RedirectToAction("ResetPasswordConfirmation");
                }
            }
            //  handle error
            return View(resetPasswordVM);
        }
        //  reset password confirmation page
        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
        //  unauthenticated
        public IActionResult Unauthenticated()
        {
            return View();
        }

    }
}
