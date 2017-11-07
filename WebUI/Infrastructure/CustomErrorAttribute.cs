using System;
using System.Text;
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
        private readonly IIpAddressProvider _ipAddressProvider;

        public CustomErrorAttribute(IExceptionLogger exceptionLogger, IRouteDataConverter routeDataConverter,
            IIpAddressProvider ipAddressProvider)
        {
            _exceptionLogger = exceptionLogger;
            _routeDataConverter = routeDataConverter;
            _ipAddressProvider = ipAddressProvider;
        }

        public void OnException(ExceptionContext filterContext)
        {
            var xForwardedFor = filterContext.HttpContext.Request.Headers["X-Forwarded-For"];
            var userHostAddress = filterContext.HttpContext.Request.UserHostAddress;
            var hostAddresses = string.IsNullOrEmpty(xForwardedFor)
                ? userHostAddress
                : $"{xForwardedFor}, {userHostAddress}";
            var allIpAddresses = GetAllIpAddresses(hostAddresses);

            var loggingModel = new MvcLoggingModel()
            {
                UserName = filterContext.HttpContext.User.Identity.Name,
                UserHostAddress = GetAllIpAddresses(allIpAddresses),
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

        private string GetAllIpAddresses(string hostAddresses)
        {
            var inAddresses = hostAddresses.Split(new char[]{','}, StringSplitOptions.RemoveEmptyEntries);
            var outAddresses = new StringBuilder();

            foreach (var ip in inAddresses)
            {
                outAddresses.Append(_ipAddressProvider.GetIpAddress(ip));
            }

            return outAddresses.ToString();
        }
    }
}