using Loggers.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Providers;
using System;
using System.Text;
using WebUI.Core.Abstract.Converters;
using Serilog;

namespace WebUI.Core.Infrastructure
{
    public class CustomErrorAttribute : IExceptionFilter
    {
        private readonly ILogger _logger = Log.Logger.ForContext<CustomErrorAttribute>();
        private readonly IRouteDataConverter _routeDataConverter;
        private readonly IMultipleIpAddressProvider _multipleIpAddressProvider;

        public CustomErrorAttribute(
            IRouteDataConverter routeDataConverter,
            IMultipleIpAddressProvider multipleIpAddressProvider
            )
        {
            _routeDataConverter = routeDataConverter;
            _multipleIpAddressProvider = multipleIpAddressProvider;
        }

        public void OnException(ExceptionContext context)
        {
            var xForwardedFor = context.HttpContext.Request.Headers["X-Forwarded-For"];
            var userHostAddress = context.HttpContext.Connection.RemoteIpAddress.ToString();
            var hostAddresses = string.IsNullOrEmpty(xForwardedFor)
                ? userHostAddress
                : $"{xForwardedFor},{userHostAddress}";
            var allIpAddresses = _multipleIpAddressProvider.GetIpAddresses(hostAddresses);

            var errorLoggingModel = new ErrorLoggingModel()
            {
                RequestId = context.HttpContext.TraceIdentifier,
                ConnectionId = context.HttpContext.Connection.Id,
                IpAddresses = allIpAddresses,
                QueryString = context.HttpContext.Request.QueryString.ToString(),
                RequestMethod = context.HttpContext.Request.Method,
                RequestPath = context.HttpContext.Request.Path,
                SessionId = context.HttpContext.Session?.Id,
                UserLogin = context.HttpContext.User.Identity.Name
            };

            _logger.Error("Error while handling request{@errorLoggingModel}", errorLoggingModel);

            if (!context.ExceptionHandled)
            {
                context.ExceptionHandled = false;
            }
        }

        private void FillInnerExceptions(StringBuilder errorMessage, Exception exception)
        {
            errorMessage.AppendLine($"Ошибка: {exception.Message}");
            errorMessage.AppendLine($"Трассировка стэка: {exception.StackTrace}");
            errorMessage.AppendLine("----------------Конец исключения----------------");
            errorMessage.AppendLine("\r\n");

            if (exception.InnerException != null)
            {
                FillInnerExceptions(errorMessage, exception.InnerException);
            }
        }
    }
}