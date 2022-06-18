using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using WebUI.Core.Infrastructure.Identity.Models;

namespace WebUI.Core.Models.RoleModels
{
    public class RoleEditModel
    {
        public IdentityRole Role { get; set; }
        public IEnumerable<AccountingUserModel> Members { get; set; }
        public IEnumerable<AccountingUserModel> NonMembers { get; set; }
    }
}
