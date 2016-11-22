using System.Web.Mvc;
using HomeAccountingSystem_WebUI.Abstract;

namespace HomeAccountingSystem_WebUI.Controllers
{
    public class ReportMenuController : Controller
    {
        private readonly IReportMenu _reportMenu;

        public ReportMenuController(IReportMenu reportMenu)
        {
            this._reportMenu = reportMenu;
        }

        public PartialViewResult Index(int id = 0)
        {
            ViewBag.ReportMenuId = id;
            return PartialView(_reportMenu);
        }
    }
}