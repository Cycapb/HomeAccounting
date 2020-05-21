using Microsoft.AspNetCore.Mvc.Filters;
using Services;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Core.Abstract;
using WebUI.Core.Infrastructure.ActionResults;
using WebUI.Core.Infrastructure.Extensions;
using WebUI.Core.Models;

namespace WebUI.Core.Infrastructure.Filters
{
    public class UserHasAnyAccount : IAsyncActionFilter
    {
        private readonly string _webUserKey = "WebUser";
        private readonly IAccountService _accService;
        private readonly IMessageProvider _messageProvider;

        public UserHasAnyAccount(IAccountService accountService, IMessageProvider messageProvider)
        {
            _accService = accountService;
            _messageProvider = messageProvider;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var session = context.HttpContext.Session;

            if (session != null)
            {
                var user = await session.GetJsonAsync<WebUser>(_webUserKey);

                if (user != null)
                {
                    var accounts = await _accService.GetListAsync(x => x.UserId == user.Id);

                    if (!accounts.Any())
                    {
                        context.Result = new UserHasNoAccountsActiontResult(_messageProvider);
                    }
                }
            }

            await next();
        }
    }
}