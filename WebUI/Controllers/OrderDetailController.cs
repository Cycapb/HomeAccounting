using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Caching;
using System.Web.SessionState;
using DomainModels.Model;
using WebUI.Models;
using Services;
using Services.Exceptions;
using WebUI.Exceptions;
using Services.Caching;
using WebUI.Models.OrderModels;

namespace WebUI.Controllers
{
    [Authorize]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class OrderDetailController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly ICacheManager _cacheManager;
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;

        public OrderDetailController(     
            ICategoryService categoryService,
            ICacheManager cacheManager,
            IOrderService orderService,
            IProductService productService)
        {
            _categoryService = categoryService;
            _cacheManager = cacheManager;
            _orderService = orderService;
            _productService = productService;
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id, int orderId)
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

        public async Task<ActionResult> Add(WebUser user, int id)
        {
            try
            {
                var categories = (IEnumerable<Category>)_cacheManager.Get("Categories");

                if (categories == null)
                {
                    categories = (await GetCategories(user.Id)).ToList();
                    _cacheManager.Set("Categories", categories, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(60));
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
            catch(Exception ex)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(OrderDetailController)} в методе {nameof(Add)}", ex);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Add(OrderDetail orderDetail)
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

        public async Task<ActionResult> GetSubCategories(int id)
        {
            var categories = (IEnumerable<Category>)_cacheManager.Get("Categories");

            if (categories == null)
            {
                var user = (WebUser)HttpContext.Session?["WebUser"];

                try
                {
                    categories = (await GetCategories(user.Id)).ToList();
                    _cacheManager.Set("Categories", categories, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(60));
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

        private async Task<IEnumerable<Category>> GetCategories(string userId)
        {
            return (await _categoryService.GetListAsync(x => x.UserId == userId && x.TypeOfFlowID == 2 && x.Products.Any()))
                .Select(x => new Category()
                {
                    CategoryID = x.CategoryID,
                    Name = x.Name,
                    Products = x.Products
                })
                .OrderBy(x => x.Name);                
        }
    }

}