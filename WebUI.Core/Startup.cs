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
using Services;

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
                options.UseSqlServer(_configuration["ConnectionStrings:AccountingEntities:ConnectionString"]);
            });
            services.AddTransient<IMailboxService, MailboxService>();
            services.AddTransient <IRepository<NotificationMailBox>, EntityRepositoryCore<NotificationMailBox, AccountingContextCore>>();
            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddMemoryCache();
            services.AddSession();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePages();
            }

            app.UseBrowserLink();
            app.UseStaticFiles();
            app.UseSession();
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
        }
    }
}
