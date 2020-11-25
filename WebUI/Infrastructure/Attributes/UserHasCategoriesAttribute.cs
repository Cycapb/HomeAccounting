using System.Linq;
using System.Web.Mvc;
using WebUI.Models;
using WebUI.Abstract;
using Services;
using WebUI.App_Start;

namespace WebUI.Infrastructure.Attributes
{
    public class UserHasCategoriesAttributeAttribute:FilterAttribute,IActionFilter
    {
        private readonly ICategoryService _categoryService;
        private readonly IMessageProvider _messageProvider;

        public UserHasCategoriesAttributeAttribute()
        {
            _categoryService = (ICategoryService)NinjectWebCommon.Kernel.GetService(typeof(ICategoryService));
            _messageProvider = (IMessageProvider)NinjectWebCommon.Kernel.GetService(typeof(IMessageProvider));
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = filterContext.HttpContext.Session;                            
            var userHasCategories = false;
            var curUser = (WebUser)session?["WebUser"];
            if (curUser != null)
            {
                if (filterContext.RequestContext.RouteData.Values.TryGetValue("typeOfFlowId", out object value))
                {
                    var tofId = int.Parse((string)value);
                    userHasCategories = _categoryService.GetList(x => x.UserId == curUser.Id && x.Active && x.TypeOfFlowID == tofId).Any();
                }
                else
                {
                    userHasCategories = _categoryService.GetList(x => x.UserId == curUser.Id && x.Active).Any();
                }

            }
            if (!userHasCategories)
            {
                filterContext.Result = new UserHasNoCategoriesActionResult(_messageProvider);
            }
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            
        }
    }
}