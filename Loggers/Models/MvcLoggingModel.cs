using System.Collections.Generic;

namespace Loggers.Models
{
    public class MvcLoggingModel
    {
        public string UserName { get; set; }
        public string UserHostAddress { get; set; }
        public Dictionary<string, object> RouteData { get; set; }
    }
}