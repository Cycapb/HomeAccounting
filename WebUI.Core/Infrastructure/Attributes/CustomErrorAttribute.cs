using Loggers;
using Loggers.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Providers;
using System.Threading.Tasks;
using WebUI.Core.Abstract.Converters;
using Serilog;
using Microsoft.Extensions.Logging;

namespace WebUI.Core.Infrastructure
{
    public class CustomErrorAttribute : IExceptionFilter
    {
        private readonly ILogger<CustomErrorAttribute> _exceptionLogger;
        private readonly IRouteDataConverter _routeDataConverter;

        public CustomErrorAttribute(
            ILogger<CustomErrorAttribute> exceptionLogger,
            IRouteDataConverter routeDataConverter
            )
        {
            _exceptionLogger = exceptionLogger;
            _routeDataConverter = routeDataConverter;            
        }

        public void OnException(ExceptionContext context)
        {
            var xForwardedFor = context.HttpContext.Request.Headers["X-Forwarded-For"];
            var userHostAddress = context.HttpContext.Connection.RemoteIpAddress.ToString();
            var hostAddresses = string.IsNullOrEmpty(xForwardedFor)
                ? userHostAddress
                : $"{xForwardedFor},{userHostAddress}";
            //var allIpAddresses = _multipleIpAddressProvider.GetIpAddresses(hostAddresses);

            //var loggingModel = new MvcLoggingModel()
            //{
            //    UserName = context.HttpContext.User.Identity.Name,
            //    UserHostAddress = allIpAddresses,
            //    RouteData = _routeDataConverter.ConvertRouteData(context.HttpContext.Request.RouteValues)
            //};
            _exceptionLogger.LogError(context.Exception,"EXCEPTION!!!");

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