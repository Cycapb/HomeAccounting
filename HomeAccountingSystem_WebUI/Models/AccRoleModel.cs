using Microsoft.AspNet.Identity.EntityFramework;

namespace WebUI.Models
{
    public class AccRoleModel:IdentityRole
    {
        public AccRoleModel() : base() { }
        public AccRoleModel(string name) : base(name) { }
    }
}