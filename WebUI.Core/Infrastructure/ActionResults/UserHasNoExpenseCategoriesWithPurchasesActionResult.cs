using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Threading.Tasks;
using WebUI.Core.Abstract;

namespace WebUI.Core.Infrastructure.ActionResults
{
    public class UserHasNoExpenseCategoriesWithPurchasesActionResult : IActionResult
    {
        private readonly IMessageProvider _messageProvider;

        public UserHasNoExpenseCategoriesWithPurchasesActionResult(IMessageProvider messageProvider)
        {
            _messageProvider = messageProvider;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status200OK;
            var response = Encoding.UTF8.GetBytes($"<div class='alert alert-danger'>{_messageProvider.Get(MessagesEnum.UserHasNoExpenseCategoriesWithPurchases)}</div>");
            await context.HttpContext.Response.Body.WriteAsync(response);
        }
    }
}
