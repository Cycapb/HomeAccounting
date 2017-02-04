using System.Web.Mvc;

namespace HomeAccountingSystem_WebUI.Infrastructure
{
    public class UserHasNoCategoriesActionResult : ActionResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.StatusCode = 200;
            context.HttpContext.Response.Write("<div class='alert alert-danger'>Сначала необходимо завести хотя бы одну категорию.</div>");
        }
    }
}