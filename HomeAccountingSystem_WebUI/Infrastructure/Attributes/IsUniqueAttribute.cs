using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using HomeAccountingSystem_WebUI.App_Start;
using Ninject;
using Services;

namespace HomeAccountingSystem_WebUI.Infrastructure.Attributes
{
    public class IsUniqueAttribute:ValidationAttribute,IClientValidatable
    {
        private readonly IMailboxService _mailboxService;

        public IsUniqueAttribute()
        {
            _mailboxService = NinjectWebCommon.Kernel.Get<IMailboxService>();
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var val = (string) value;

            var any = _mailboxService.GetList().Any(x => x.MailBoxName == val);

            if (any)
            {
                var result = new ValidationResult(this.ErrorMessage = validationContext.DisplayName);
                return result;
            }

            return ValidationResult.Success;
        }

        //Todo Дописать валидацию со стороны jquery
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            yield return new ModelClientValidationRule()
            {
                ErrorMessage = this.ErrorMessage,
                ValidationType = "IsUnique"
            };
        }
    }
}