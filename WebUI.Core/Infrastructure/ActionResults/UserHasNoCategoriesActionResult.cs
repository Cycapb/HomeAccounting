using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Threading.Tasks;
using WebUI.Core.Abstract;

namespace WebUI.Core.Infrastructure
{
    public class UserHasNoCategoriesActionResult : IActionResult
    {
        private readonly IMessageProvider _messageProvider;

        public UserHasNoCategoriesActionResult(IMessageProvider messageProvider)
        {
            _messageProvider = messageProvider;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            context.HttpContext.Response.StatusCode = 200;
            var body = Encoding.UTF8.GetBytes($"<div class='alert alert-danger'>{_messageProvider.Get(MessagesEnum.UserHasNoCategories)}</div>");

            await context.HttpContext.Response.Body.WriteAsync(body);
        }
    }
}