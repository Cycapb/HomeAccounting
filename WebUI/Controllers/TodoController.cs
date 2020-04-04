using System.Web.Mvc;
using System.Web.SessionState;
using WebUI.Models;
using Microsoft.AspNet.Identity;

namespace WebUI.Controllers
{
    [Authorize]
    [SessionState(SessionStateBehavior.Disabled)]
    public class TodoController : Controller
    {
        public ActionResult Index()
        {
            var model = new TodoModel()
            {
                UserId = HttpContext.User.Identity.GetUserId()
            };
            return PartialView("_Index", model);
        }
    }
}