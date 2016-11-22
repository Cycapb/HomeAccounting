using HomeAccountingSystem_WebUI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace HomeAccountingSystem_WebUI.Infrastructure
{
    public class AccUserManager:UserManager<AccUserModel>
    {
        public AccUserManager(IUserStore<AccUserModel> store) : base(store)
        {
        }

        public static AccUserManager Create(IdentityFactoryOptions<AccUserManager> options, IOwinContext context)
        {
            AccIdentityDbContext db = context.Get<AccIdentityDbContext>();
            AccUserManager manager = new AccUserManager(new UserStore<AccUserModel>(db));
            manager.PasswordValidator = new CustomPasswordValidator()
            {
                RequiredLength = 6,
                RequireDigit = true,
            };
            manager.UserValidator = new CustomUserValidator(manager)
            {
                AllowOnlyAlphanumericUserNames = true,
                RequireUniqueEmail = true
            };
            return manager;
        }
    }
}