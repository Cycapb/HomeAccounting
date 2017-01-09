using System.Web.Mvc;

namespace HomeAccountingSystem_WebUI.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult UserHasNoCategory()
        {
            return PartialView();
        }
    }
}