using System.Web.Mvc;
using System.Web.SessionState;

namespace HomeAccountingSystem_WebUI.Controllers
{
    [Authorize(Roles = "Administrators")]
    [SessionState(SessionStateBehavior.Disabled)]
    public class AdminSectionController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}