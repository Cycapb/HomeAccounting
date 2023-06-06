using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using WebUI.Core.Exceptions;
using WebUI.Core.Infrastructure.Identity.Models;

namespace WebUI.Core.Infrastructure.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static async Task<IHtmlContent> GetNameAsync(this HtmlHelper helper, IServiceProvider serviceProvider, string id)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<AccountingUserModel>>();

            if (userManager != null)
            {
                var user = await userManager.FindByIdAsync(id);

                if (user != null)
                {
                    return new HtmlString(user.UserName);
                }
            }

            throw new WebUiException($"Couldn't receive required service of type {nameof(UserManager<AccountingUserModel>)}");
        }
    }
}