using Serilog;
using Serilog.Configuration;
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;

namespace Loggers.Extensions.Serilog.Enrichers
{
    public static class HttpContextLoggerConfigurationExtension
    {
        public static LoggerConfiguration WithHttpContextProperties(
            this LoggerEnrichmentConfiguration enrichmentConfiguration, 
            IServiceProvider serviceProvider)
        {
            if (enrichmentConfiguration == null) throw new ArgumentNullException(nameof(enrichmentConfiguration));
            var httpContextAccessor = serviceProvider.GetService<IHttpContextAccessor>();

            return enrichmentConfiguration.With(new HttpContextEnricher(httpContextAccessor));
        }
    }
}
