using Microsoft.AspNetCore.Http;
using Serilog;

namespace WebUI.Core.Infrastructure.Extensions
{
    public static class SerilogContextEnricherExtension
    {
        public static ILogger EnrichLoggerFromHttpContext(this ILogger logger, HttpContext context)
        {            
            return logger
                .ForContext("RequestId", context.TraceIdentifier)
                .ForContext("ConnectionId", context.Connection.Id)
                .ForContext("QueryString", context.Request.QueryString.ToString())
                .ForContext("RequestMethod", context.Request.Method)
                .ForContext("RequestPath", context.Request.Path)
                .ForContext("SessionId", context.Session?.Id)
                .ForContext("UserLogin", context.User.Identity.Name);
        }
    }
}
