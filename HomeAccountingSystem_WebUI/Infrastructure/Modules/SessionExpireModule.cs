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
            if (HttpContext.Current.CurrentNotification != RequestNotification.AcquireRequestState) return;
            var ctx = HttpContext.Current;
            var session = ctx.Session;
            if (session == null) return;
            if (ctx.User.Identity.IsAuthenticated && session["WebUser"] == null)
            {
                var auth = ctx.GetOwinContext().Authentication;
                auth.SignOut();
                ctx.Response.Redirect("/", true);
            }
        }

        public void Dispose()
        {
                
        }
    }
}