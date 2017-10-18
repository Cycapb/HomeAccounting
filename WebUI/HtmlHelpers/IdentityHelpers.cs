using System.Web;
using System.Web.Mvc;
using WebUI.Infrastructure;
using Microsoft.AspNet.Identity.Owin;

namespace WebUI.HtmlHelpers
{
    public static class IdentityHelpers
    {
        public static MvcHtmlString GetName(this HtmlHelper helper,string id)
        {
            return new MvcHtmlString(HttpContext.Current.GetOwinContext().GetUserManager<AccUserManager>().FindByIdAsync(id).Result.UserName);
        }
    }
}