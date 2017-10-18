using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebUI.Infrastructure.Binders;
using WebUI.Models;

namespace WebUI
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
