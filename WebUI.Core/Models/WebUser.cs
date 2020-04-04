using DomainModels.Model;

namespace WebUI.Core.Models
{
    public class WebUser : IWorkingUser
    {
        public string Name { get; set; }
        
        public string Id { get; set; }

        public string Email { get; set; }
    }
}