using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Threading.Tasks;
using WebUI.Core.Infrastructure.Extensions;
using WebUI.Core.Models;

namespace WebUI.Core.Infrastructure.Binders
{
    public class WebUserBinder : IModelBinder
    {
        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext is null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var user = await bindingContext.HttpContext.Session.GetJsonAsync<WebUser>("WebUser");
            user ??= CreateDefaultUserWithNameApostol();
            
            bindingContext.Result = ModelBindingResult.Success(user);
            return;
        }

        private WebUser CreateDefaultUserWithNameApostol()
        {
            return new WebUser()
            {
                Email = "aka.apostol@gmail.com",
                Name = "Apostol",
                Id = "c4b81c0d-8843-437a-8730-95b1641eeb7d"
            };
        }
    }
}
