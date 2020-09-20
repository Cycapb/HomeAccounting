using Microsoft.AspNetCore.Mvc;

namespace WebUI.Core.Components
{
    public class LoginPage : ViewComponent
    {
        public IViewComponentResult Invoke(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View("/Views/UserAccount/_Login.cshtml");
        }
    }
}
