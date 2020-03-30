using Loggers;
using Loggers.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Providers;
using System.Threading.Tasks;
using WebUI.Core.Abstract.Converters;

namespace WebUI.Core.Infrastructure
{
    public class CustomErrorAttribute : IExceptionFilter
    {
        private readonly IExceptionLogger _exceptionLogger;
        private readonly IRouteDataConverter _routeDataConverter;
        private readonly IMultipleIpAddressProvider _multipleIpAddressProvider;

        public CustomErrorAttribute(
            IExceptionLogger exceptionLogger,
            IRouteDataConverter routeDataConverter,
            IMultipleIpAddressProvider multipleIpAddressProvider)
        {
            _exceptionLogger = exceptionLogger;
            _routeDataConverter = routeDataConverter;
            _multipleIpAddressProvider = multipleIpAddressProvider;
        }

        public void OnException(ExceptionContext context)
        {
            var xForwardedFor = context.HttpContext.Request.Headers["X-Forwarded-For"];
            //var userHostAddress = context.HttpContext.Request.UserHostAddress;
            var hostAddresses = xForwardedFor; //string.IsNullOrEmpty(xForwardedFor)
            //    ? userHostAddress
            //    : $"{xForwardedFor},{userHostAddress}";
            var allIpAddresses = _multipleIpAddressProvider.GetIpAddresses(hostAddresses);

            var loggingModel = new MvcLoggingModel()
            {
                UserName = context.HttpContext.User.Identity.Name,
                UserHostAddress = allIpAddresses,
                RouteData = _routeDataConverter.ConvertRouteData(context.HttpContext.Request.RouteValues)
            };
            _exceptionLogger.LogException(context.Exception, loggingModel);

            if (!context.ExceptionHandled)
            {
                context.Result = new ViewResult()
                {
                    ViewName = "ErrorPage"
                };

                context.ExceptionHandled = true;
            }
        }
    }
}