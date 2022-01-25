using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Services;
using System.Threading.Tasks;
using WebUI.Core.Controllers;

namespace WebUI.Core.Tests.Controllers
{
    [TestClass]
    public class OrderControllerTests
    {
        private readonly Mock<IOrderService> _orderServiceMock;

        private readonly Mock<ILogger<OrderController>> _loggerMock;

        public OrderControllerTests()
        {
            _orderServiceMock = new Mock<IOrderService>();
            _loggerMock = new Mock<ILogger<OrderController>>();
        }

        [TestMethod]
        [TestCategory("OrderControllerTests")]
        public async Task CloseOrder()
        {
            var target = new OrderController(_orderServiceMock.Object, _loggerMock.Object);

            await target.CloseOrder(1);

            _orderServiceMock.Verify(m => m.CloseOrderAsync(It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        [TestCategory("OrderControllerTests")]
        public async Task CloseOrderReturnsRedirectToRouteResult()
        {
            var target = new OrderController(_orderServiceMock.Object, _loggerMock.Object);

            var result = await target.CloseOrder(1);

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
        }
    }
}
