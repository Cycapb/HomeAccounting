using BussinessLogic.Services;
using DomainModels.EntityORM.Core.Infrastructure;
using DomainModels.Model;
using DomainModels.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using WebUI.Core.Infrastructure.Middleware;

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

            services.AddTransient<IMailboxService, MailboxService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IRepository<NotificationMailBox>, EntityRepositoryCore<NotificationMailBox, AccountingContextCore>>();
            services.AddTransient<IRepository<Category>, EntityRepositoryCore<Category, AccountingContextCore>>();
            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromDays(14);
            });
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
            app.UseSerilogRequestLogging();
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

            MigrateAndSeed(app);
        }

        private void MigrateAndSeed(IApplicationBuilder app)
        {
            var context = app.ApplicationServices.GetRequiredService<AccountingContextCore>();
            context.Database.Migrate();

            InitializeNotificationMailBox(context);
            InitializeTypeOfFlow(context);

            context.SaveChanges();
        }

        private void InitializeNotificationMailBox(AccountingContextCore context)
        {
            if (!context.NotificationMailBoxes.Any())
            {
                var mailBox = new NotificationMailBox()
                {
                    MailBoxName = "Accounting",
                    MailFrom = "home.accounting@list.ru",
                    UserName = "home.accounting@list.ru",
                    Password = "23we45rt",
                    UseSsl = true,
                    Server = "smtp.list.ru",
                    Port = 587
                };

                context.NotificationMailBoxes.Add(mailBox);
            }
        }

        private void InitializeTypeOfFlow(AccountingContextCore context)
        {
            if (!context.TypeOfFlows.Any())
            {
                var typesOfFlow = new List<TypeOfFlow>()
                {
                    new TypeOfFlow()
                    {
                        TypeName = "Доход"
                    },
                     new TypeOfFlow()
                     {
                         TypeName = "Расход"
                     }
                };
                context.TypeOfFlows.AddRange(typesOfFlow);
            }
        }

        private void InitializeSerilog(IServiceProvider serviceProvider)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(_configuration)
                .CreateLogger();
        }
    }
}
