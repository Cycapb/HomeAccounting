using System.Web.Mvc;
using System.Web.SessionState;

namespace WebUI.Controllers
{
    [Authorize(Roles = "Administrators")]
    [SessionState(SessionStateBehavior.Disabled)]
    public class AdminSectionController : Controller
    {
        public ActionResult Index()
        {
            return PartialView("_Index");
        }
    }
}