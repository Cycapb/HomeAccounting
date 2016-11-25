using System.Web.Mvc;

namespace HomeAccountingSystem_WebUI.Controllers
{
    public class MainBarController : Controller
    {
        [OutputCache(Duration = 120)]
        public ActionResult Index()
        {
            return PartialView("_MainBar");
        }
    }
}