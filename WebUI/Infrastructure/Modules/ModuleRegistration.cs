using System.Web;
using WebUI.Infrastructure.Modules;

[assembly: PreApplicationStartMethod(typeof(ModuleRegistration),"RegisterModule")]

namespace WebUI.Infrastructure.Modules
{
    public class ModuleRegistration
    {
        public static void RegisterModule()
        {
            HttpApplication.RegisterModule(typeof(SessionExpireModule));
        }
    }
}