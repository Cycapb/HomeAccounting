using Microsoft.AspNetCore.Identity;

namespace WebUI.Core.Infrastructure.Identity.Models
{
    public class AccountingRoleModel : IdentityRole
    {
        public AccountingRoleModel() : base() { }

        public AccountingRoleModel(string name) : base(name) { }
    }
}