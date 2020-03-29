using Microsoft.AspNetCore.Identity;

namespace WebUI.Core.Models
{
    public class AccRoleModel : IdentityRole
    {
        public AccRoleModel() : base() { }
        public AccRoleModel(string name) : base(name) { }
    }
}