using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using HomeAccountingSystem_WebUI.Infrastructure.Binders;
using HomeAccountingSystem_WebUI.Models;

namespace HomeAccountingSystem_WebUI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ModelBinders.Binders.Add(typeof(WebUser),new WebUserModelBinder());
        }
    }
}
