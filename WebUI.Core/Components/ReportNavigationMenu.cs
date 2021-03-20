using Microsoft.AspNetCore.Mvc;
using WebUI.Core.Models.ReportModels;

namespace WebUI.Core.Components
{
    public class ReportNavigationMenu : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var reportModel = new ReportMenu();

            return View(reportModel);
        }
    }
}
