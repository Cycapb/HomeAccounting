using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace WebUI.Infrastructure
{
    public class CustomPasswordValidator:PasswordValidator
    {
        public override async Task<IdentityResult> ValidateAsync(string item)
        {
            Regex reg = new Regex(@"\d{3,}");
            IdentityResult result = await base.ValidateAsync(item);
            var errors = result.Errors.ToList();
            if (reg.IsMatch(item))
            {
                errors.Add("В пароле не может быть последовательности из более, чем 2-х цифр");
                result = new IdentityResult(errors);
            }
            return result;
        }
    }
}