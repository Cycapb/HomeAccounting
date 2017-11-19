using System.Web.Mvc;
using Converters;
using Loggers;
using WebUI.App_Start;
using WebUI.Infrastructure;
using Providers;

namespace WebUI
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new CustomErrorAttribute(
                (IExceptionLogger) NinjectWebCommon.Kernel.GetService(typeof(IExceptionLogger)),
                (IRouteDataConverter) NinjectWebCommon.Kernel.GetService(typeof(IRouteDataConverter)),
                (IMultipleIpAddressProvider) NinjectWebCommon.Kernel.GetService(typeof(IMultipleIpAddressProvider))
                ));
        }
    }
}