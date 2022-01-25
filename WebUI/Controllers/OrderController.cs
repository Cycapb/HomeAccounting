using DomainModels.Model;
using Services;
using Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.SessionState;
using WebUI.Exceptions;
using WebUI.Infrastructure.Attributes;
using WebUI.Models;

namespace WebUI.Controllers
{
    [Authorize]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<ActionResult> Index(WebUser user)
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

        public async Task<ActionResult> OrderList(WebUser user)
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
        public async Task<ActionResult> Delete(int id)
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

        public async Task<ActionResult> Edit(int id)
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
        [UserHasCategories]
        public async Task<ActionResult> Add(WebUser user)
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

                return RedirectToAction("Edit", new { id = createdOrder.OrderID});
            }
            catch (ServiceException e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(OrderController)} в методе {nameof(Add)}", e);
            }
        }

        [HttpPost]
        public async Task SendEmail(int id)
        {
            var mailTo = ((WebUser)Session["WebUser"]).Email;
            if (mailTo != null)
            {
                try
                {
                    await Task.Run(() => _orderService.SendByEmailAsync(id, ((WebUser)Session["WebUser"]).Email));
                }
                catch (ServiceException e)
                {
                    throw new WebUiException($"Ошибка в контроллере {nameof(OrderController)} в методе {nameof(SendEmail)}", e);
                }
            }
        }

        [HttpPost]
        public async Task<ActionResult> CloseOrder(int id)
        {
            try
            {
                await _orderService.CloseOrderAsync(id);
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