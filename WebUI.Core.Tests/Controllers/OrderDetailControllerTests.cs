using DomainModels.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Services;
using System.Threading.Tasks;
using WebUI.Core.Controllers;

namespace WebUI.Core.Tests.Controllers
{
    [TestClass]
    public class OrderDetailControllerTests
    {

        private readonly OrderDetailController _target;
        private readonly Mock<ICategoryService> _categoryServiceMock;
        private readonly Mock<IMemoryCache> _cacheMock;
        private readonly Mock<IProductService> _productServiceMock;
        private readonly Mock<IOrderService> _orderServiceMock;

        public OrderDetailControllerTests()
        {
            _categoryServiceMock = new Mock<ICategoryService>();
            _cacheMock = new Mock<IMemoryCache>();
            _productServiceMock = new Mock<IProductService>();
            _orderServiceMock = new Mock<IOrderService>();
            _target = new OrderDetailController(_categoryServiceMock.Object, _cacheMock.Object, _orderServiceMock.Object, _productServiceMock.Object);
        }

        [TestMethod]
        [TestCategory("OrderDetailControllerTests")]
        public async Task DeleteReturnsRedirectToAction()
        {
            _orderServiceMock.Setup(m => m.GetItemAsync(It.IsAny<int>())).ReturnsAsync(new Order());

            var result = await _target.Delete(It.IsAny<int>(), It.IsAny<int>());

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
        }

        [TestMethod]
        [TestCategory("OrderDetailControllerTests")]
        public async Task DeleteReturnsRedirectWithParameterId()
        {
            var orderId = 1;
            _orderServiceMock.Setup(m => m.GetItemAsync(It.IsAny<int>())).ReturnsAsync(new Order() { OrderID = 1 });

            var result = await _target.Delete(It.IsAny<int>(), orderId);

            Assert.AreEqual(orderId, ((RedirectToActionResult)result).RouteValues["id"]);
        }
    }
}
