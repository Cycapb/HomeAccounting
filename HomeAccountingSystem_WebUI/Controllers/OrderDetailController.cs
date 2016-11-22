using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Caching;
using System.Web.SessionState;
using HomeAccountingSystem_DAL.Model;
using HomeAccountingSystem_WebUI.Models;
using Services;

namespace HomeAccountingSystem_WebUI.Controllers
{
    [Authorize]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class OrderDetailController : Controller
    {
        private readonly IOrderDetailService _orderDetailService;
        private readonly ICategoryService _categoryService;

        public OrderDetailController(IOrderDetailService orderDetailService, ICategoryService categoryService)
        {
            _orderDetailService = orderDetailService;
            _categoryService = categoryService;
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            var orderId = (await _orderDetailService.GetItemAsync(id)).OrderId;
            await _orderDetailService.DeleteAsync(id);
            return RedirectToAction("Edit", "Order", new {id = orderId});
        }

        public async Task<ActionResult> Add(WebUser user, int id)
        {
            var categories = (await GetCategories(user.Id)).ToList();
            HttpContext.Cache.Insert("Categories", categories, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(60));
            var model = new AddOrderDetailView()
            {
                OrderId = id,
                Categories = categories,
                Products = categories.FirstOrDefault()?.Product
            };
            return PartialView("_Add", model);
        }

        [HttpPost]
        public async Task<ActionResult> Add(OrderDetail orderDetail)
        {
            await _orderDetailService.CreateAsync(orderDetail);
            return RedirectToAction("Edit", "Order", new {id = orderDetail.OrderId});
        }

        public async Task<ActionResult> GetSubCategories(int id)
        {
            var categories = (IEnumerable<Category>)HttpContext.Cache.Get("Categories");
            if (categories == null)
            {
                var user = (WebUser)HttpContext.Session?["WebUser"];
                categories = await GetCategories(user.Id);
            }
            var products = categories.FirstOrDefault(x => x.CategoryID == id)?.Product;
            return PartialView("_SubCategories", products);
        }

        private async Task<IEnumerable<Category>> GetCategories(string userId)
        {
            return (await _categoryService.GetListAsync()).Where(x => x.UserId == userId && x.TypeOfFlowID == 2 && x.Product.Any()).ToList();
        }
    }

}