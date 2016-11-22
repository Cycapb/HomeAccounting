using System.Web;
using System.Web.Mvc;
using HomeAccountingSystem_WebUI.Infrastructure;
using Microsoft.AspNet.Identity.Owin;

namespace HomeAccountingSystem_WebUI.HtmlHelpers
{
    public static class IdentityHelpers
    {
        public static MvcHtmlString GetName(this HtmlHelper helper,string id)
        {
            return new MvcHtmlString(HttpContext.Current.GetOwinContext().GetUserManager<AccUserManager>().FindByIdAsync(id).Result.UserName);
        }
    }
}