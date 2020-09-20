using DomainModels.Model;
using Microsoft.AspNetCore.Mvc;
using WebUI.Core.Infrastructure.Binders;

namespace WebUI.Core.Models
{
    [ModelBinder(typeof(WebUserBinder))]
    public class WebUser : IWorkingUser
    {
        public string Name { get; set; }
        
        public string Id { get; set; }

        public string Email { get; set; }
    }
}