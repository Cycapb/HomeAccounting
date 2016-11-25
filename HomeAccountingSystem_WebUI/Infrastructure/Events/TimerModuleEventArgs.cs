using System;

namespace HomeAccountingSystem_WebUI.Infrastructure.Events
{
    public class TimerModuleEventArgs:EventArgs
    {
        public float Duration { get; set; }
    }
}