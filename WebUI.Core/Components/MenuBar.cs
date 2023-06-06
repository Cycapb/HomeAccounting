using Microsoft.AspNetCore.Mvc;

namespace WebUI.Core.Components
{
    public class MenuBar : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
