using System.Web.Mvc;
using HomeAccountingSystem_WebUI.Abstract;

namespace HomeAccountingSystem_WebUI.Infrastructure
{
    public class UserHasNoAccountsActiontResult : ActionResult
    {
        IMessageProvider _messageProvider;

        public UserHasNoAccountsActiontResult(IMessageProvider messageProvider)
        {
            _messageProvider = messageProvider;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.StatusCode = 200;
            context.HttpContext.Response.Write($"<div class='alert alert-danger'>{_messageProvider.Get(MessagesEnum.UserHasNoAccounts)}</div>");
        }
    }
}