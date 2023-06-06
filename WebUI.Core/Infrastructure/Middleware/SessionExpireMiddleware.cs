using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using WebUI.Core.Infrastructure.Extensions;
using WebUI.Core.Infrastructure.Identity.Models;
using WebUI.Core.Models;

namespace WebUI.Core.Infrastructure.Middleware
{
    public class SessionExpireMiddleware
    {
        private readonly RequestDelegate _requestDelegate;
        private readonly UserManager<AccountingUserModel> _userManager;

        public SessionExpireMiddleware(RequestDelegate requestDelegate, UserManager<AccountingUserModel> userManager)
        {
            _requestDelegate = requestDelegate;
            _userManager = userManager;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var webUser = await httpContext.Session.GetJsonAsync<WebUser>(nameof(WebUser));

            if (webUser == null && httpContext.User.Identity.IsAuthenticated)
            {
                var user = await _userManager.FindByNameAsync(httpContext.User.Identity.Name);
                var newWebUser = new WebUser()
                {
                    Name = user.FirstName,
                    Email = user.Email,
                    Id = user.Id
                };
                await httpContext.Session.SetJsonAsync<WebUser>(nameof(WebUser), newWebUser);
            }

            await _requestDelegate.Invoke(httpContext);
        }
    }
}
