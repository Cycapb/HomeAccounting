using System.Linq;
using System.Web.Mvc;
using HomeAccountingSystem_WebUI.Models;
using Services;
using BussinessLogic.Services;
using DomainModels.EntityORM;
using DomainModels.Model;

namespace HomeAccountingSystem_WebUI.Infrastructure.Attributes
{
    public class UserHasAnyAccountAttribute : FilterAttribute, IActionFilter
    {
        private IAccountService _accService;

        public UserHasAnyAccountAttribute()
        {
            _accService = new AccountService(new EntityRepository<Account>());
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
                    filterContext.Result = new UserHasNoAccountsActiontResult();
                }
            }
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            
        }
    }
}