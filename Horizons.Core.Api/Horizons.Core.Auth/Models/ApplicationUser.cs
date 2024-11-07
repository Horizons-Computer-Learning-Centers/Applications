using Microsoft.AspNetCore.Identity;

namespace Horizons.Core.Auth.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
