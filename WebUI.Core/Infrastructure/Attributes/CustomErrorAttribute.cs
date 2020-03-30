using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Providers;
using System;
using System.Text;
using WebUI.Core.Abstract.Converters;

namespace WebUI.Core.Infrastructure
{
    public class CustomErrorAttribute : IExceptionFilter
    {
        private readonly ILogger<CustomErrorAttribute> _logger;
        private readonly IRouteDataConverter _routeDataConverter;
        private readonly IMultipleIpAddressProvider _multipleIpAddressProvider;

        public CustomErrorAttribute(
            ILogger<CustomErrorAttribute> logger,
            IRouteDataConverter routeDataConverter,
            IMultipleIpAddressProvider multipleIpAddressProvider
            )
        {
            _logger = logger;
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