using WebUI.Infrastructure;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

namespace WebUI
{
    public class AccIdentityConfig
    {
        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext<AccIdentityDbContext>(AccIdentityDbContext.Create);
            app.CreatePerOwinContext<AccUserManager>(AccUserManager.Create);
            app.CreatePerOwinContext<AccRoleManager>(AccRoleManager.Create);
            app.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/UserAccount/Login")
            });
        }
    }
}