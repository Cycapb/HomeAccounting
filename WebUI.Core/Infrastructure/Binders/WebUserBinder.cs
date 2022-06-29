using Microsoft.AspNetCore.Mvc.ModelBinding;
using Serilog;
using System;
using System.Threading.Tasks;
using WebUI.Core.Infrastructure.Extensions;
using WebUI.Core.Models;

namespace WebUI.Core.Infrastructure.Binders
{
    public class WebUserBinder : IModelBinder
    {
        private readonly ILogger _logger = Log.Logger.ForContext<WebUserBinder>();

        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext is null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var user = await bindingContext.HttpContext.Session.GetJsonAsync<WebUser>("WebUser");

            if (user == null)
            {
                _logger.Error("Не удалось получить данные пользователя из сессии");

                throw new ArgumentNullException(nameof(user));
            }

            bindingContext.Result = ModelBindingResult.Success(user);
        }
    }
}
