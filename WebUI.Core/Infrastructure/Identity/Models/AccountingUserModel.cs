using Microsoft.AspNetCore.Identity;

namespace WebUI.Core.Infrastructure.Identity.Models
{
    public class AccountingUserModel : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
