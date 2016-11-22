using System.Linq;
using System.Threading.Tasks;
using HomeAccountingSystem_WebUI.Models;
using Microsoft.AspNet.Identity;

namespace HomeAccountingSystem_WebUI.Infrastructure
{
    public class CustomUserValidator:UserValidator<AccUserModel>
    {
        public CustomUserValidator(AccUserManager manager) : base(manager)
        {
        }

        public override async Task<IdentityResult> ValidateAsync(AccUserModel item)
        {
            IdentityResult result = await base.ValidateAsync(item);
            if (item.Email.ToLower().Contains("@bn1.su"))
            {
                var errors = result.Errors.ToList();
                errors.Add("Почтовый домен не может быть bn1.su");
                result = new IdentityResult(errors);
            }
            return result;
        }
    }
}