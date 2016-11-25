using System.Linq;
using System.Web;

namespace HomeAccountingSystem_WebUI.Infrastructure.Modules
{
    public class TotalTimeModule:IHttpModule
    {
        private static float _time;

        public void Init(HttpApplication context)
        {
            var moduleName = HttpContext.Current.ApplicationInstance.Modules.AllKeys.FirstOrDefault(x => x.Contains("TimerModule"));
            var timerModule = HttpContext.Current.ApplicationInstance.Modules[moduleName];
            if (timerModule != null)
            {
                var module = new TimerModule();
                module = (TimerModule)timerModule;
                module.RequestEnd += Module_RequestEnd; 
            }
        }

        private void Module_RequestEnd(object sender, Events.TimerModuleEventArgs e)
        {
            _time += e.Duration;
            HttpContext.Current.Response.Write($"<div class='alert alert-success'>Elapsed overall time: {_time}</div>");
            _time = 0;
        }

        public void Dispose()
        {
            
        }
    }
}