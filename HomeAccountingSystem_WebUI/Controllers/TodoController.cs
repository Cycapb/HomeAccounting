using System.Web.Mvc;
using System.Web.SessionState;
using HomeAccountingSystem_WebUI.Models;
using Microsoft.AspNet.Identity;

namespace HomeAccountingSystem_WebUI.Controllers
{
    [Authorize]
    [SessionState(SessionStateBehavior.Disabled)]
    public class TodoController : Controller
    {
        public ActionResult Index()
        {
            var model = new TodoViewModel()
            {
                UserId = HttpContext.User.Identity.GetUserId()
            };
            return View(model);
        }
    }
}