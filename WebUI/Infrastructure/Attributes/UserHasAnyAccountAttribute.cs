using System.Linq;
using System.Web.Mvc;
using WebUI.Models;
using WebUI.Abstract;
using Services;
using WebUI.App_Start;

namespace WebUI.Infrastructure.Attributes
{
    public class UserHasAnyAccountAttribute : FilterAttribute, IActionFilter
    {
        private readonly IAccountService _accService;
        private readonly IMessageProvider _messageProvider;

        public UserHasAnyAccountAttribute()
        {
            _accService = (IAccountService)NinjectWebCommon.Kernel.GetService(typeof(IAccountService));
            _messageProvider = (IMessageProvider)NinjectWebCommon.Kernel.GetService(typeof(IMessageProvider));
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var session = filterContext.HttpContext.Session;
            var user = (WebUser)session?["WebUser"];
            if (user != null)
            {
                var accounts = _accService.GetList(x => x.UserId == user.Id);
                if (accounts.Any())
                {
                    filterContext.Result = new UserHasNoAccountsActiontResult(_messageProvider);
                }
            }
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            
        }
    }
}