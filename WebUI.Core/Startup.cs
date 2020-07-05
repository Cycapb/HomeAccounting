using DomainModels.EntityORM.Core.Infrastructure;
using Loggers.Extensions.Serilog.Enrichers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using WebUI.Core.Infrastructure;
using WebUI.Core.Infrastructure.Identity;
using WebUI.Core.Infrastructure.Identity.Models;
using WebUI.Core.Infrastructure.Identity.Validators;
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
            services.AddTransient<IPasswordValidator<AccountingUserModel>, CustomPasswordValidator>();

            services.AddDbContext<AccountingContextCore>(options =>
            {
                options.UseLazyLoadingProxies();
                options.UseSqlServer(_configuration["ConnectionStrings:AccountingEntities:ConnectionString"]);
            }, ServiceLifetime.Transient);

            services.AddDbContext<AccountingIdentityDbContext>(options =>
            {
                options.UseSqlServer(_configuration["ConnectionStrings:AccountingIdentity:ConnectionString"]);
            });

            services.AddIdentity<AccountingUserModel, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<AccountingIdentityDbContext>()
                .AddDefaultTokenProviders();
            services.ConfigureApplicationCookie(options => options.LoginPath = "/UserAccount/Index");

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
                app.UseBrowserLink();
            }

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
            app.UseAuthentication();
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
                routes.MapRoute("Default", "{controller}/{action}/{id?}", new { controller = "PayingItem", action = "Index" });
            });

            DatabaseMigrator.MigrateDatabaseAndSeed(app);
            DatabaseMigrator.MigrateIdentityDatabaseAndSeed(app).Wait();
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
