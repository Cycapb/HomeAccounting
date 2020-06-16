using Microsoft.AspNetCore.Mvc;

namespace WebUI.Core.Components
{
    public class NavLeft : ViewComponent
    {
        public IViewComponentResult Invoke()
        {            
            return View("/Views/NavLeft/Accounts.cshtml");
        }
    }
}
