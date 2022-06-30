using Loggers.Extensions.Serilog.Models;
using Microsoft.AspNetCore.Http;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;

namespace Loggers.Extensions.Serilog.Enrichers
{
    public class HttpContextEnricher : ILogEventEnricher
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextEnricher() : this(new HttpContextAccessor())
        {

        }

        public HttpContextEnricher(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            if (logEvent == null) throw new ArgumentNullException("logEvent");


            if (_httpContextAccessor?.HttpContext == null)
            {
                return;
            }

            var logEventProperties = CreateLogEventProperties();

            foreach (var logEventProperty in logEventProperties)
            {
                logEvent.AddPropertyIfAbsent(logEventProperty);
            }
        }

        private List<LogEventProperty> CreateLogEventProperties()
        {
            return new List<LogEventProperty>()
            {
                new LogEventProperty(LoggingProperties.ConnectionId, new ScalarValue(_httpContextAccessor.HttpContext.Connection.Id)),
                new LogEventProperty(LoggingProperties.RequestId, new ScalarValue(_httpContextAccessor.HttpContext.TraceIdentifier)),
                new LogEventProperty(LoggingProperties.QueryString, new ScalarValue(_httpContextAccessor.HttpContext.Request.QueryString)),
                new LogEventProperty(LoggingProperties.RequestMethod, new ScalarValue(_httpContextAccessor.HttpContext.Request.Method)),
                new LogEventProperty(LoggingProperties.RequestPath, new ScalarValue(_httpContextAccessor.HttpContext.Request.Path)),
                new LogEventProperty(LoggingProperties.SessionId, new ScalarValue(_httpContextAccessor.HttpContext.Session?.Id)),
                new LogEventProperty(LoggingProperties.UserName, new ScalarValue(_httpContextAccessor.HttpContext.User.Identity?.Name)),
            };
        }
    }
}
