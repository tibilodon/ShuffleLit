using Microsoft.AspNetCore.Identity;

namespace ShuffleLit.Models
{
    public class AppUser : IdentityUser
    {
        public string? ProfileImageUrl { get; set; }

    }
}
