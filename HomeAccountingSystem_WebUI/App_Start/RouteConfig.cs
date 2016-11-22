using System.Web.Mvc;
using System.Web.Mvc.Routing.Constraints;
using System.Web.Routing;

namespace HomeAccountingSystem_WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("", "",
                new
                {
                    controller = "PayingItem",
                    action = "Index"
                }
                );


            routes.MapRoute("Report", "Report/{action}", new
            {
                controller = "Report",
                action = "Index"
            });


            routes.MapRoute("", "Report/{action}/{typeOfFlowId}", new
            {
                controller = "Report",
                action = "SubcategoriesReport",
                typeOfFlowId = UrlParameter.Optional
            });

            routes.MapRoute("Page", "Page{page}",
                new
                {
                    controller = "PayingItem",
                    action = "List",
                    page = 1
                },
                new {page = @"\d"});

            routes.MapRoute("","{typeOfFlow}",
                new
                {
                    controller = "PayingItem",
                    action = "Add"
                },
                new
                {
                    typeOfFlow = new RangeRouteConstraint(1,2)
                });

            routes.MapRoute("", "Category/Edit/{id}",
                new
                {
                    controller = "Category" ,
                    action = "Edit"
                });

            routes.MapRoute("", "Category/Delete/{id}",
                new
                {
                    controller = "Category",
                    action = "Delete"
                });

            routes.MapRoute("", "Category/{action}/{typeOfFlowId}/{page}",
                new { controller = "Category" });

            routes.MapRoute("", "Category/{action}/{page}",
                new { controller = "Category" , action = "Index"});



            routes.MapRoute("EditPayingItem", "{controller}/{action}/{typeOfFlowId}/{id}",
                new
                {
                    controller = "PayingItem",
                    action = "EditPayingItem"
                });

            routes.MapRoute("","Todo/{action}",new {controller = "Todo",action = "Index"});

            routes.MapRoute("Default", "{controller}/{action}/{id}",
                new
                {
                    action = "Index",
                    id = UrlParameter.Optional
                });
        }
    }
}