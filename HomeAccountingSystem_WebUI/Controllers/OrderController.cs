using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.SessionState;
using BussinessLogic.Services;
using HomeAccountingSystem_WebUI.Models;
using Services;

namespace HomeAccountingSystem_WebUI.Controllers
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
            var orders = (await _orderService.GetList(user.Id)).Where(u => u.Active).ToList();
            return PartialView("_Index",orders);
        }

        public async Task<ActionResult> OrderList(WebUser user)
        {
            var orders = (await _orderService.GetList(user.Id)).Where(u => u.Active).ToList();
            return PartialView("_OrderList", orders);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            await  _orderService.DeleteAsync(id);
            return RedirectToAction("OrderList");
        }

        public async Task<ActionResult> Edit(int id)
        {
            var order = await _orderService.GetOrderAsync(id);
            return PartialView("_OrderDetailsList", order);
        }

        [HttpPost]
        public async Task<ActionResult> Add(WebUser user)
        {
            await _orderService.CreateOrderAsync(DateTime.Today, user.Id);
            return RedirectToAction("OrderList");
        }

        [HttpPost]
        public async Task SendEmail(int id)
        {
            EmailSenderService.MailTo = ((WebUser)Session["WebUser"]).Email;
            await Task.Run(() => _orderService.SendByEmail(id));   
        }

        [HttpPost]
        public async Task<ActionResult> CloseOrder(int id)
        {
            await _orderService.CloseOrder(id);
            return RedirectToAction("OrderList");
        }
    }
}