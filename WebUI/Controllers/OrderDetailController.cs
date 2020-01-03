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

namespace WebUI.Controllers
{
    [Authorize]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class OrderDetailController : Controller
    {
        private readonly IOrderDetailService _orderDetailService;
        private readonly ICategoryService _categoryService;
        private readonly ICacheManager _cacheManager;

        public OrderDetailController(
            IOrderDetailService orderDetailService,
            ICategoryService categoryService,
            ICacheManager cacheManager)
        {
            _orderDetailService = orderDetailService;
            _categoryService = categoryService;
            _cacheManager = cacheManager;
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            int orderId;
            try
            {
                orderId = (await _orderDetailService.GetItemAsync(id)).OrderId;
                await _orderDetailService.DeleteAsync(id);
            }
            catch (ServiceException e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(OrderDetailController)} в методе {nameof(Delete)}", e);
            }
            return RedirectToAction("Edit", "Order", new {id = orderId});
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
                
                var model = new AddOrderDetailView()
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
        }

        [HttpPost]
        public async Task<ActionResult> Add(OrderDetail orderDetail)
        {
            try
            {
                await _orderDetailService.CreateAsync(orderDetail);
            }
            catch (ServiceException e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(OrderDetailController)} в методе {nameof(Add)}", e);
            }
            return RedirectToAction("Edit", "Order", new {id = orderDetail.OrderId});
        }

        public async Task<ActionResult> GetSubCategories(int id)
        {
            var categories = (IEnumerable<Category>)_cacheManager.Get("Categories");

            if (categories == null)
            {
                var user = (WebUser)HttpContext.Session?["WebUser"];

                try
                {
                    categories = await GetCategories(user.Id);
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

        private async Task<IEnumerable<Category>> GetCategories(string userId)
        {
            return (await _categoryService.GetListAsync(x => x.UserId == userId && x.TypeOfFlowID == 2 && x.Products.Any()))
                .OrderBy(x => x.Name)
                .ToList();
        }
    }

}