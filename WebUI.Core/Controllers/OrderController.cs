using DomainModels.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Exceptions;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Core.Exceptions;
using WebUI.Core.Infrastructure.Extensions;
using WebUI.Core.Infrastructure.Filters;
using WebUI.Core.Models;

namespace WebUI.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<IActionResult> Index(WebUser user)
        {
            try
            {
                var orders = (await _orderService.GetListAsync(x => x.UserId == user.Id && x.Active)).ToList();
                return PartialView("_Index", orders);
            }
            catch (ServiceException e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(OrderController)} в методе {nameof(Index)}", e);
            }
        }

        public async Task<IActionResult> OrderList(WebUser user)
        {
            try
            {
                var orders = (await _orderService.GetListAsync(o => o.UserId == user.Id && o.Active)).ToList();

                return PartialView("_OrderList", orders);
            }
            catch (ServiceException e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(OrderController)} в методе {nameof(OrderList)}", e);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _orderService.DeleteAsync(id);
            }
            catch (ServiceException e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(OrderController)} в методе {nameof(Delete)}", e);
            }

            return RedirectToAction("OrderList");
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var order = await _orderService.GetItemAsync(id);

                return PartialView("_OrderDetailsList", order);
            }
            catch (ServiceException e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(OrderController)} в методе {nameof(Edit)}", e);
            }
        }

        [HttpPost]
        [TypeFilter(typeof(UserHasCategories))]
        public async Task<IActionResult> Add(WebUser user)
        {
            try
            {
                var order = new Order()
                {
                    OrderDate = DateTime.Now,
                    UserId = user.Id,
                    Active = true
                };

                var createdOrder = await _orderService.CreateAsync(order);

                return RedirectToAction("Edit", new { id = createdOrder.OrderID });
            }
            catch (ServiceException e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(OrderController)} в методе {nameof(Add)}", e);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task SendEmail(int id)
        {
            var mailTo = (await HttpContext.Session.GetJsonAsync<WebUser>(nameof(WebUser)))?.Email;

            if (mailTo != null)
            {
                try
                {
                    await _orderService.SendByEmail(id, mailTo);
                }
                catch (ServiceException e)
                {
                    throw new WebUiException($"Ошибка в контроллере {nameof(OrderController)} в методе {nameof(SendEmail)}", e);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CloseOrder(int id)
        {
            try
            {
                await _orderService.CloseOrder(id);
            }
            catch (ServiceException e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(OrderController)} в методе {nameof(CloseOrder)}", e);
            }

            return RedirectToAction("OrderList");
        }

        protected override void Dispose(bool disposing)
        {
            _orderService.Dispose();

            base.Dispose(disposing);
        }
    }
}