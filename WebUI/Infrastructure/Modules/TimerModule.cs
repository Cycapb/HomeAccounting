using System;
using System.Diagnostics;
using System.Web;
using WebUI.Infrastructure.Events;

namespace WebUI.Infrastructure.Modules
{
    public class TimerModule:IHttpModule
    {
        private readonly Stopwatch _stopwatch = new Stopwatch();

        public event EventHandler<TimerModuleEventArgs> RequestEnd; 

        public void Init(HttpApplication context)
        {
            context.BeginRequest += Context_BeginRequest;
            context.EndRequest += Context_EndRequest;
        }

        private void Context_EndRequest(object sender, EventArgs e)
        {
            if (HttpContext.Current.CurrentNotification == RequestNotification.EndRequest)
            {
                float elapsedTime = (float)_stopwatch.ElapsedTicks / Stopwatch.Frequency;
                _stopwatch.Reset();
                RequestEnd?.Invoke(this, new TimerModuleEventArgs() {Duration = elapsedTime});
            }
        }

        private void Context_BeginRequest(object sender, EventArgs e)
        {
            if (HttpContext.Current.CurrentNotification == RequestNotification.BeginRequest)
            {
               _stopwatch.Start();
            }    
        }

        public void Dispose()
        {
            
        }
    }
}