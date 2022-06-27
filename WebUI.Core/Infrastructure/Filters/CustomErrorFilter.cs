using Microsoft.AspNetCore.Mvc.Filters;
using Providers;
using Serilog;
using System;
using System.Text;

namespace WebUI.Core.Infrastructure.Filters
{
    public class CustomErrorFilter : IExceptionFilter
    {
        private readonly ILogger _logger = Log.Logger.ForContext<CustomErrorFilter>();
        private readonly IMultipleIpAddressProvider _multipleIpAddressProvider;

        public CustomErrorFilter(IMultipleIpAddressProvider multipleIpAddressProvider)
        {
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
            var errorMessage = new StringBuilder();
            FillInnerExceptions(errorMessage, context.Exception);

            _logger
                .ForContext("IpAddresses", allIpAddresses)
                .ForContext("Full_Stack_Trace", errorMessage)
                .Error("Error while handling request");

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