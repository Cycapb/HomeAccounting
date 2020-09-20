using System;

namespace WebUI.Infrastructure.Events
{
    public class TimerModuleEventArgs:EventArgs
    {
        public float Duration { get; set; }
    }
}