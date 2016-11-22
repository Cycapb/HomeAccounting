using Microsoft.AspNet.Identity.EntityFramework;

namespace HomeAccountingSystem_WebUI.Models
{
    public class AccUserModel:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}