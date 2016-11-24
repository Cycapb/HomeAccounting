using System.Web;
using HomeAccountingSystem_WebUI.Infrastructure.Modules;

[assembly: PreApplicationStartMethod(typeof(ModuleRegistration),"RegisterModule")]

namespace HomeAccountingSystem_WebUI.Infrastructure.Modules
{
    public class ModuleRegistration
    {
        public static void RegisterModule()
        {
            HttpApplication.RegisterModule(typeof(SessionExpireModule));
        }
    }
}