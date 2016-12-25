using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BussinessLogic.Services;
using DomainModels.EntityORM;
using HomeAccountingSystem_DAL.Model;
using HomeAccountingSystem_WebUI.Models;
using Services;

namespace HomeAccountingSystem_WebUI.Infrastructure
{
    public class UserHasAnyCategoriesAttribute:ActionFilterAttribute
    {
        private readonly ICategoryService _categoryService;

        public UserHasAnyCategoriesAttribute()
        {
            _categoryService = new CategoryService(new EntityRepository<Category>());
        }

        public override async void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = filterContext.HttpContext.Session;
            var userHasCategories = false;
            var curUser = (WebUser) session?["WebUser"];
            if (curUser != null)
            {
                userHasCategories = (await _categoryService.GetListAsync()).Any(x => x.UserId == curUser.Id);
            }
            if (!userHasCategories)
            {
                
                //filterContext.Result = new ContentResult()
                //{
                //    Content = "<div class='alert alert-danger'>У вас нет ни одной категории.Сначала необходимо добавить хотя бы одну.</div>"
                //};
                var url = UrlHelper.GenerateUrl("", "List", "PayingItem", filterContext.RequestContext.RouteData.Values,
                    RouteTable.Routes, filterContext.RequestContext, false);
                filterContext.HttpContext.Response.Redirect(url);
    }
        }
    }
}