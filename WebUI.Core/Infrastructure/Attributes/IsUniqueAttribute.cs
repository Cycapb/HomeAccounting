using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.DependencyInjection;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebUI.Core.Infrastructure.Attributes
{
    public class IsUniqueAttribute : Attribute, IModelValidator
    {
        private readonly string _errorMessage = "Такой почтовый ящик уже существует";

        public string ErrorMessage { get; set; }

        public IEnumerable<ModelValidationResult> Validate(ModelValidationContext context)
        {
            var mailboxService = context.ActionContext.HttpContext.RequestServices.GetService<IMailboxService>();
            var val = (string)context.Model;

            var mailBoxes = mailboxService.GetList(x => x.MailBoxName == val);
            var validationResults = new List<ModelValidationResult>();

            if (mailBoxes.Any())
            {
                var errorMessage = !string.IsNullOrWhiteSpace(ErrorMessage) ? ErrorMessage : _errorMessage;
                validationResults.Add(new ModelValidationResult("", errorMessage));

                return validationResults;
            }

            return validationResults;
        }
    }
}