﻿using Microsoft.AspNetCore.Mvc;

namespace WebUI.Core.Components
{
    public class MainBar : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
