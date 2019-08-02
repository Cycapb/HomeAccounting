using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.App_Start;
using Ninject;
using Services;

namespace WebUI.Infrastructure.Attributes
{
    public class IsUniqueAttribute: ValidationAttribute
    {
        private readonly IMailboxService _mailboxService;
        public string Mailboxname { get; private set; }

        public IsUniqueAttribute()
        {
            _mailboxService = NinjectWebCommon.Kernel.Get<IMailboxService>();
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var val = (string)value;
            Mailboxname = val;

            if ((string)HttpContext.Current.Request.RequestContext.RouteData.Values["action"] != "Edit")
            {
                var mailBoxes = _mailboxService.GetList(x => x.MailBoxName == val);

                if (mailBoxes.Any())
                {
                    var result = new ValidationResult(ErrorMessage = ErrorMessageString);
                    return result;
                }
            }

            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule()
            {
                ErrorMessage = ErrorMessageString,
                ValidationType = "isunique"
            };

            rule.ValidationParameters["mailboxname"] = Mailboxname;

            yield return rule;
        }
    }
}