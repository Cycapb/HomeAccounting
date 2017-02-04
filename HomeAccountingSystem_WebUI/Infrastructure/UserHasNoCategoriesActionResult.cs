using System.Web.Mvc;

namespace HomeAccountingSystem_WebUI.Infrastructure
{
    public class UserHasNoCategoriesActionResult : ActionResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.StatusCode = 200;
            context.HttpContext.Response.Write("<div class='alert alert-danger'>У вас нет ни одной категории.Сначала необходимо добавить хотя бы одну.</div>");
        }
    }
}