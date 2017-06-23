using System.Threading.Tasks;
using System.Web.Mvc;
using DomainModels.Model;
using DomainModels.Repositories;
using WebUI.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Services;

namespace WebUI.Tests.ControllerTests
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
        [TestCategory("OrderControllerTests")]
        public async Task CloseOrder()
        {
            var target = new OrderController(_orderService.Object);
            _orderRepo.Setup(m => m.GetItemAsync(It.IsAny<int>())).ReturnsAsync(new Order());

            await target.CloseOrder(1);

            _orderService.Verify(m => m.CloseOrder(It.IsAny<int>()),Times.Once);
        }

        [TestMethod]
        [TestCategory("OrderControllerTests")]
        public async Task CloseOrderReturnsRedirectToRouteResult()
        {
            var target = new OrderController(_orderService.Object);
            _orderRepo.Setup(m => m.GetItemAsync(It.IsAny<int>())).ReturnsAsync(new Order());

            var result = await target.CloseOrder(1);

            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }
    }
}
