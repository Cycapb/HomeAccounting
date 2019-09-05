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

namespace WebUI.Controllers
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
            AddOrderDetailView model;
            try
            {
                var categories = (await GetCategories(user.Id)).ToList();
                HttpContext.Cache.Insert("Categories", categories, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(60));
                model = new AddOrderDetailView()
                {
                    OrderId = id,
                    Categories = categories,
                    Products = categories.FirstOrDefault()?.Products ?? new List<Product>()
                };
            }
            catch (ServiceException e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(OrderDetailController)} в методе {nameof(Add)}", e);
            }
            return PartialView("_Add", model);
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
            var categories = (IEnumerable<Category>)HttpContext.Cache.Get("Categories");
            if (categories == null)
            {
                var user = (WebUser)HttpContext.Session?["WebUser"];
                try
                {
                    categories = await GetCategories(user.Id);
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
            return (await _categoryService.GetListAsync())
                .Where(x => x.UserId == userId && x.TypeOfFlowID == 2 && x.Products.Any())
                .OrderBy(x => x.Name)
                .ToList();
        }
    }

}