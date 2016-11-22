using System.Web.Mvc;
using HomeAccountingSystem_WebUI.Models;
using IModelBinder = System.Web.Mvc.IModelBinder;
using ModelBindingContext = System.Web.Mvc.ModelBindingContext;

namespace HomeAccountingSystem_WebUI.Infrastructure.Binders
{
    public class WebUserModelBinder:IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            WebUser user = null;
            if (controllerContext.HttpContext.Session != null)
            {
                var webUser = (WebUser)controllerContext.HttpContext.Session["WebUser"];
                if (webUser != null)
                {
                    user = webUser;
                }
            }
            return user;
        }
    }
}