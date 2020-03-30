namespace Loggers.Models
{
    public class BaseLoggingModel
    {
        public string UserLogin { get; set; }

        public string SessionId { get; set; }

        public string IpAddresses { get; set; }

        public string RequestId { get; set; }

        public string ConnectionId { get; set; }

        public string RequestMethod { get; set; }

        public string RequestPath { get; set; }

        public string QueryString { get; set; }
    }
}
