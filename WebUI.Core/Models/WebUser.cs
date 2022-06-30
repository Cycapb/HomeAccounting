using Microsoft.AspNetCore.Mvc;
using WebUI.Core.Infrastructure.Binders;

namespace WebUI.Core.Models
{
    [ModelBinder(typeof(WebUserBinder))]
    public class WebUser
    {
        public string Name { get; set; }

        public string Id { get; set; }

        public string Email { get; set; }
    }
}