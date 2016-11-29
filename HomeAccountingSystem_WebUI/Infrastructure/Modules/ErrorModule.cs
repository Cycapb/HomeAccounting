using System.Web;

namespace HomeAccountingSystem_WebUI.Infrastructure.Modules
{
    public class ErrorModule:IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.Error += (src, obj) =>
            {
                context.Response.Write($"Error: {context.Context.Error}");
            };
        }

        public void Dispose()
        {
            
        }
    }
}