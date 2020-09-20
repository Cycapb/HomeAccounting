using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebUI.Core.Models;

namespace WebUI.Core.Infrastructure.Identity.Validators
{
    public class CustomPasswordValidator : PasswordValidator<AccountingUserModel>
    {
        public override async Task<IdentityResult> ValidateAsync(UserManager<AccountingUserModel> manager, AccountingUserModel user, string password)
        {
            Regex reg = new Regex(@"\d{3,}");
            IdentityResult result = await base.ValidateAsync(manager, user, password);
            var errors = result.Errors.ToList();

            if (reg.IsMatch(password))
            {
                errors.Add(new IdentityError() { Code = "PasswordContainsTowDigitsInARow", Description = "В пароле не может быть последовательности из более, чем 2-х цифр" });
            }

            return errors.Count == 0 ? IdentityResult.Success : IdentityResult.Failed(errors.ToArray());
        }
    }
}
