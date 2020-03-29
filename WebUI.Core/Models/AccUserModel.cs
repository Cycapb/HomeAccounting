using Microsoft.AspNetCore.Identity;

namespace WebUI.Core.Models
{
    public class AccUserModel : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}