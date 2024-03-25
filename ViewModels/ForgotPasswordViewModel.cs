using System.ComponentModel.DataAnnotations;

namespace ShuffleLit.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email address")]
        public string Email { get; set; }
        public DateTime? PasswordChangedDate { get; set; }


    }
}
