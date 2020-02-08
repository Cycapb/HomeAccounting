using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WebUI.Core
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
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
                routes.MapRoute("Report", "Report/{action}", new { controller = "Report", action = "Index"});
                routes.MapRoute("", "Report/{action}/{typeOfFlowId?}", new { controller = "Report", action = "SubcategoriesReport"});
                routes.MapRoute("Page", "Page{page}", new { controller = "PayingItem", action = "List", page = 1 }, new { page = @"\d"});
                routes.MapRoute("", "{typeOfFlowId:range(1,2)}", new { controller = "PayingItem", action = "Add" });
                routes.MapRoute("", "Category/{action}/{id}", new {controller = "Category"});
                routes.MapRoute("", "Category/{action}/{typeOfFlowId}/{page}", new { controller = "Category"});
                routes.MapRoute("", "Category/{action}/{page}", new { controller = "Category"});
                routes.MapRoute("", "Todo/{action}", new { controller = "Todo", action = "Index"});
                routes.MapRoute("Default", "{controller}/{action}/{id?}", new { action = "Index"});
            });
        }
    }
}
