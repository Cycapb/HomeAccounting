using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using HomeAccountingSystem_DAL.Model;
using HomeAccountingSystem_DAL.Repositories;
using HomeAccountingSystem_WebUI.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Services;

namespace WebUI.Tests
{
    [TestClass]
    public class OrderControllerTests
    {
        private readonly Mock<IOrderService> _orderService;
        private readonly Mock<IRepository<Order>> _orderRepo;

        public OrderControllerTests()
        {
            _orderService = new Mock<IOrderService>();
            _orderRepo = new Mock<IRepository<Order>>();
        }

        [TestMethod]
        public async Task CloseOrder()
        {
            var target = new OrderController(_orderService.Object);
            _orderRepo.Setup(m => m.GetItemAsync(It.IsAny<int>())).ReturnsAsync(new Order());

            await target.CloseOrder(1);

            _orderService.Verify(m => m.CloseOrder(It.IsAny<int>()),Times.Once);
        }

        [TestMethod]
        public async Task CloseOrderReturnsRedirectToRouteResult()
        {
            var target = new OrderController(_orderService.Object);
            _orderRepo.Setup(m => m.GetItemAsync(It.IsAny<int>())).ReturnsAsync(new Order());

            var result = await target.CloseOrder(1);

            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }
    }
}
