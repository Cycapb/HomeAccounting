using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using WebUI.Core.Models.UserModels;

namespace WebUI.Core.Models.RoleModels
{
    public class RoleEditModel
    {
        public RoleModel Role { get; set; }
        public IEnumerable<UserModel> Members { get; set; }
        public IEnumerable<UserModel> NonMembers { get; set; }
    }
}
