using Microsoft.AspNetCore.Identity;

namespace ShuffleLit.Models
{
    public class AppUser : IdentityUser
    {
        //public string? NickName { get; set; }
        public string? ProfileImageUrl { get; set; }
        public DateTime? PasswordChangedDate { get; set; }
    }
}
