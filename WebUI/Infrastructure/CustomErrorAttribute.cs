using System.Web.Mvc;
using Converters;
using Loggers;
using Loggers.Models;
using Providers;

namespace WebUI.Infrastructure
{
    public class CustomErrorAttribute : FilterAttribute, IExceptionFilter
    {
        private readonly IExceptionLogger _exceptionLogger;
        private readonly IRouteDataConverter _routeDataConverter;
        private readonly IMultipleIpAddressProvider _multipleIpAddressProvider;

        public CustomErrorAttribute(IExceptionLogger exceptionLogger, IRouteDataConverter routeDataConverter,
            IMultipleIpAddressProvider multipleIpAddressProvider)
        {
            _exceptionLogger = exceptionLogger;
            _routeDataConverter = routeDataConverter;
            _multipleIpAddressProvider = multipleIpAddressProvider;
        }

        public void OnException(ExceptionContext filterContext)
        {
            var xForwardedFor = filterContext.HttpContext.Request.Headers["X-Forwarded-For"];
            var userHostAddress = filterContext.HttpContext.Request.UserHostAddress;
            var hostAddresses = string.IsNullOrEmpty(xForwardedFor)
                ? userHostAddress
                : $"{xForwardedFor},{userHostAddress}";
            var allIpAddresses = _multipleIpAddressProvider.GetIpAddresses(hostAddresses);

            var loggingModel = new MvcLoggingModel()
            {
                UserName = filterContext.HttpContext.User.Identity.Name,
                UserHostAddress = allIpAddresses,
                RouteData = _routeDataConverter.ConvertRouteData(filterContext.HttpContext.Request.RequestContext
                    .RouteData.Values)
            };
            _exceptionLogger.LogException(filterContext.Exception, loggingModel);

            if (!filterContext.ExceptionHandled)
            {
                filterContext.Result = new ViewResult()
                {
                    ViewName = "ErrorPage"
                };
                filterContext.ExceptionHandled = true;
            }
        }
    }
}