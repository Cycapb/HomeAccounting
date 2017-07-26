using System.Threading.Tasks;
using System.Web.Mvc;
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

        public OrderControllerTests()
        {
            _orderService = new Mock<IOrderService>();
        }

        [TestMethod]
        [TestCategory("OrderControllerTests")]
        public async Task CloseOrder()
        {
            var target = new OrderController(_orderService.Object, null);            

            await target.CloseOrder(1);

            _orderService.Verify(m => m.CloseOrder(It.IsAny<int>()),Times.Once);
        }

        [TestMethod]
        [TestCategory("OrderControllerTests")]
        public async Task CloseOrderReturnsRedirectToRouteResult()
        {
            var target = new OrderController(_orderService.Object, null);            

            var result = await target.CloseOrder(1);

            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }
    }
}
