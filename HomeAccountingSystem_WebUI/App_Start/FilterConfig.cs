using System.Web.Mvc;
using HomeAccountingSystem_WebUI.Infrastructure;

namespace HomeAccountingSystem_WebUI
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new CustomErrorAttribute());
        }
    }
}