using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Threading.Tasks;
using WebUI.Core.Abstract;

namespace WebUI.Core.Infrastructure.ActionResults
{
    public class UserHasNoAccountsActiontResult : IActionResult
    {
        private readonly IMessageProvider _messageProvider;

        public UserHasNoAccountsActiontResult(IMessageProvider messageProvider)
        {
            _messageProvider = messageProvider;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status200OK;
            var body = Encoding.UTF8.GetBytes($"<div class='alert alert-danger'>{_messageProvider.Get(MessagesEnum.UserHasNoAccounts)}</div>");

            await context.HttpContext.Response.Body
                .WriteAsync(body);
        }
    }
}