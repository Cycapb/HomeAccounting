using DomainModels.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Services;
using Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Core.Exceptions;
using WebUI.Core.Infrastructure.Extensions;
using WebUI.Core.Models;
using WebUI.Core.Models.OrderModels;

namespace WebUI.Core.Controllers
{
    [Authorize]
    public class OrderDetailController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IMemoryCache _cache;
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;

        public OrderDetailController(
            ICategoryService categoryService,
            IMemoryCache cache,
            IOrderService orderService,
            IProductService productService)
        {
            _categoryService = categoryService;
            _cache = cache;
            _orderService = orderService;
            _productService = productService;
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, int orderId)
        {
            try
            {
                var order = await _orderService.GetItemAsync(orderId);
                var orderDetail = order.OrderDetails.FirstOrDefault(x => x.ID == id);

                if (orderDetail != null)
                {
                    order.OrderDetails.Remove(orderDetail);
                    await _orderService.UpdateAsync(order);
                }

                return RedirectToAction("Edit", "Order", new { id = orderId });
            }
            catch (ServiceException e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(OrderDetailController)} в методе {nameof(Delete)}", e);
            }
        }

        public async Task<IActionResult> Add(WebUser user, int id)
        {
            try
            {
                var cachedCategoriesKey = $"Categories_{id}_{user.Id}";
                var categories = (IEnumerable<Category>)_cache.Get(cachedCategoriesKey);

                if (categories == null)
                {
                    categories = (await GetCategoriesAsync(user.Id)).ToList();
                    _cache.Set(cachedCategoriesKey, categories, TimeSpan.FromSeconds(60));
                }

                var model = new OrderDetailModel()
                {
                    OrderId = id,
                    Categories = categories,
                    Products = categories.FirstOrDefault()?.Products ?? new List<Product>()
                };

                return PartialView("_Add", model);
            }
            catch (ServiceException e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(OrderDetailController)} в методе {nameof(Add)}", e);
            }
            catch (Exception ex)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(OrderDetailController)} в методе {nameof(Add)}", ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(OrderDetail orderDetail)
        {
            try
            {
                var order = await _orderService.GetItemAsync(orderDetail.OrderId);
                orderDetail.ProductPrice = (await _productService.GetItemAsync(orderDetail.ProductId)).PayingItemProducts.LastOrDefault()?.Price;
                order.OrderDetails.Add(orderDetail);
                await _orderService.UpdateAsync(order);

                return RedirectToAction("Edit", "Order", new { id = orderDetail.OrderId });
            }
            catch (ServiceException e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(OrderDetailController)} в методе {nameof(Add)}", e);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetSubCategories(int id)
        {
            var user = await HttpContext.Session.GetJsonAsync<WebUser>(nameof(WebUser));
            var cachedCategoriesKey = $"Categories_{id}_{user.Id}";
            var categories = (IEnumerable<Category>)_cache.Get(cachedCategoriesKey);

            if (categories == null)
            {
                try
                {
                    categories = (await GetCategoriesAsync(user.Id)).ToList();
                    _cache.Set(cachedCategoriesKey, categories, TimeSpan.FromSeconds(60));
                }
                catch (ServiceException e)
                {
                    throw new WebUiException($"Ошибка в контроллере {nameof(OrderDetailController)} в методе {nameof(GetSubCategories)}", e);
                }
            }

            var products = categories.FirstOrDefault(x => x.CategoryID == id)?.Products.OrderBy(x => x.ProductName);
            return PartialView("_SubCategories", products);
        }

        protected override void Dispose(bool disposing)
        {
            _categoryService.Dispose();
            _orderService.Dispose();
            _productService.Dispose();

            base.Dispose(disposing);
        }

        private async Task<IEnumerable<Category>> GetCategoriesAsync(string userId)
        {
            return (await _categoryService.GetListAsync(x => x.UserId == userId && x.TypeOfFlowID == 2 && x.Products.Any()))
                .OrderBy(x => x.Name);
        }
    }

}