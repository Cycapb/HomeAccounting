using Microsoft.AspNetCore.Mvc;

namespace WebUI.Core.Components
{
    public class MainPage : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
