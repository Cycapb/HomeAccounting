using BussinessLogic.Services;
using DomainModels.EntityORM.Core.Infrastructure;
using DomainModels.Model;
using DomainModels.Repositories;
using Loggers.Extensions.Serilog.Enrichers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Services;
using System;
using WebUI.Core.Abstract.Converters;
using WebUI.Core.Concrete.Converters;
using WebUI.Core.Infrastructure;
using WebUI.Core.Infrastructure.Middleware;
using WebUI.Core.Infrastructure.Migrators;

namespace WebUI.Core
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AccountingContextCore>(options =>
            {
                options.UseLazyLoadingProxies();
                options.UseSqlServer(_configuration["ConnectionStrings:AccountingEntities:ConnectionString"]);
            });

            services.AddMvc().AddMvcOptions(options =>
            {
                options.Filters.AddService<CustomErrorAttribute>();
                options.EnableEndpointRouting = false;
            });
            services.AddMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromDays(14);
            });

            ServicesRegister.RegisterAdditionalServices(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            InitializeSerilog(app.ApplicationServices);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePages();
            }

            app.UseBrowserLink();
            app.UseStaticFiles();
            app.UseSession();
            app.UseMiddleware<SessionExpireMiddleware>();
            app.UseSerilogRequestLogging(options =>
            {
                options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
                {
                    diagnosticContext.Set("ConnectionId", httpContext.Connection.Id);
                    diagnosticContext.Set("RequestId", httpContext.TraceIdentifier);
                    diagnosticContext.Set("RequestMethod", httpContext.Request.Method);
                    diagnosticContext.Set("RequestPath", httpContext.Request.Path);
                    diagnosticContext.Set("SessionId", httpContext.Session.Id);
                    diagnosticContext.Set("UserName", httpContext.User.Identity.Name);
                };
            });
            app.UseMvc(routes =>
            {
                routes.MapRoute("Report", "Report/{action}", new { controller = "Report", action = "Index" });
                routes.MapRoute("", "Report/{action}/{typeOfFlowId?}", new { controller = "Report", action = "SubcategoriesReport" });
                routes.MapRoute("Page", "Page{page}", new { controller = "PayingItem", action = "List", page = 1 }, new { page = @"\d" });
                routes.MapRoute("", "{typeOfFlowId:range(1,2)}", new { controller = "PayingItem", action = "Add" });
                routes.MapRoute("", "Category/{action}/{id}", new { controller = "Category" });
                routes.MapRoute("", "Category/{action}/{typeOfFlowId}/{page}", new { controller = "Category" });
                routes.MapRoute("", "Category/{action}/{page}", new { controller = "Category" });
                routes.MapRoute("", "Todo/{action}", new { controller = "Todo", action = "Index" });
                routes.MapRoute("Default", "{controller}/{action}/{id?}", new { action = "Index" });
            });

            DatabaseMigrator.MigrateAndSeed(app);
        }

        private void InitializeSerilog(IServiceProvider serviceProvider)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.WithHttpContextProperties(serviceProvider)
                .ReadFrom.Configuration(_configuration)
                .CreateLogger();
        }
    }
}