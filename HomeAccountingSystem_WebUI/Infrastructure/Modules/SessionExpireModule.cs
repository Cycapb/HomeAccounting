using System;
using System.Web;

namespace HomeAccountingSystem_WebUI.Infrastructure.Modules
{
    public class SessionExpireModule:IHttpModule
    {
        public void Init(HttpApplication app)
        {
            app.PostAcquireRequestState += HandleEvent;
        }

        private void HandleEvent(object sender, EventArgs e)
        {
            var ctx = HttpContext.Current;
            if (ctx.CurrentNotification == RequestNotification.AcquireRequestState)
            {
                var session = ctx.Session;
                if (session == null)
                {
                    var auth = ctx.GetOwinContext().Authentication;
                    auth.SignOut();
                    ctx.Response.Redirect("/", true);
                }
            }
        }

        public void Dispose()
        {
                
        }
    }
}