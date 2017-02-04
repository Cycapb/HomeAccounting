using System.Linq;
using System.Web.Mvc;
using BussinessLogic.Services;
using DomainModels.EntityORM;
using HomeAccountingSystem_DAL.Model;
using HomeAccountingSystem_WebUI.Models;
using Services;

namespace HomeAccountingSystem_WebUI.Infrastructure
{
    public class UserHasAnyCategoriesAttribute:FilterAttribute,IActionFilter
    {
        private readonly ICategoryService _categoryService;

        public UserHasAnyCategoriesAttribute()
        {
            _categoryService = new CategoryService(new EntityRepository<Category>());
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = filterContext.HttpContext.Session;
            var userHasCategories = false;
            var curUser = (WebUser)session?["WebUser"];
            if (curUser != null)
            {
                userHasCategories = ( _categoryService.GetList()).Any(x => x.UserId == curUser.Id && x.Active == true);
            }
            if (!userHasCategories)
            {
                filterContext.Result = new UserHasNoCategoriesActionResult();
            }
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            
        }
    }
}