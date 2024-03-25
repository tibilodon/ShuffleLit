using System.ComponentModel.DataAnnotations;

namespace ShuffleLit.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress]
        [Display(Name = "Email address")]
        public string EmailAddress { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
        //  redirect url
        public string? ReturnUrl { get; set; }
        public DateTime? PasswordChangedDate { get; set; }
    }
}
