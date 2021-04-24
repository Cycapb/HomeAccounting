using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Core.Controllers
{
    [Authorize(Roles = "Administrators")]
    public class AdminSectionController : Controller
    {
        public IActionResult Index()
        {
            return PartialView("_Index");
        }
    }
}