using System.Linq;
using System.Web.Mvc;
using WebUI.Models;
using WebUI.Abstract;
using WebUI.Concrete;
using Services;
using BussinessLogic.Services;
using DomainModels.EntityORM;
using DomainModels.Model;

namespace WebUI.Infrastructure.Attributes
{
    public class UserHasAnyAccountAttribute : FilterAttribute, IActionFilter
    {
        private IAccountService _accService;
        private IMessageProvider _messageProvider;

        public UserHasAnyAccountAttribute()
        {
            _accService = new AccountService(new EntityRepository<Account>());
            _messageProvider = new MessageProvider();
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var session = filterContext.HttpContext.Session;
            var user = (WebUser)session?["WebUser"];
            if (user != null)
            {
                var anyAccounts = _accService.GetList().Any(x => x.UserId == user.Id);
                if (!anyAccounts)
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