using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace WebUI.Core.Infrastructure.Middleware
{
    public class SessionExpireMiddleware
    {
        private readonly RequestDelegate _requestDelegate;

        public SessionExpireMiddleware(RequestDelegate requestDelegate)
        {
            _requestDelegate = requestDelegate;
        }

        public async Task Invoke(HttpContext httpContext) => await _requestDelegate.Invoke(httpContext);
    }
}
