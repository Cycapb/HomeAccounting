namespace Loggers.Extensions.Serilog.Models
{
    public static class LoggingProperties
    {
        public static string RequestId => "RequestId";

        public static string ConnectionId => "ConnectionId";

        public static string QueryString => "QueryString";

        public static string RequestMethod => "RequestMethod";

        public static string RequestPath => "RequestPath";

        public static string SessionId => "SessionId";

        public static string UserName => "UserName";
    }
}
