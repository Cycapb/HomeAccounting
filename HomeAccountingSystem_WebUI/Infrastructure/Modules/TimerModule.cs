using System;
using System.Diagnostics;
using System.Web;

namespace HomeAccountingSystem_WebUI.Infrastructure.Modules
{
    public class TimerModule:IHttpModule
    {
        private readonly Stopwatch _stopwatch;
        public void Init(HttpApplication context)
        {
            context.BeginRequest += Context_BeginRequest;
            context.EndRequest += Context_EndRequest;
        }

        private void Context_EndRequest(object sender, EventArgs e)
        {
            if (HttpContext.Current.CurrentNotification == RequestNotification.EndRequest)
            {
                var elapsedTime = _stopwatch.ElapsedTicks / Stopwatch.Frequency;
                HttpContext.Current.Response.Write($"<div class='alert alert-success'>Общее время: {elapsedTime}</div>");
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