using System.Web.Mvc;
using WebUI.Abstract;

namespace WebUI.Controllers
{
    public class ReportMenuController : Controller
    {
        private readonly IReportMenu _reportMenu;

        public ReportMenuController(IReportMenu reportMenu)
        {
            _reportMenu = reportMenu;
        }

        public PartialViewResult Index(int id = 0)
        {
            ViewBag.ReportMenuId = id;
            return PartialView(_reportMenu);
        }
    }
}