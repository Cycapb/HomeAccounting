using System.Threading.Tasks;
using System.Web.Mvc;
using DomainModels.Model;
using WebUI.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Services;
using Services.Caching;

namespace WebUI.Tests.ControllersTests
{
    [TestClass]
    public class OrderDetailControllerTests
    {
        
        private readonly OrderDetailController _target;
        private readonly Mock<ICategoryService> _categoryServiceMock;
        private readonly Mock<ICacheManager> _cacheManagerMock;
        private readonly Mock<IProductService> _productServiceMock;
        private readonly Mock<IOrderService> _orderServiceMock;

        public OrderDetailControllerTests()
        {
            _categoryServiceMock = new Mock<ICategoryService>();
            _cacheManagerMock = new Mock<ICacheManager>();
            _productServiceMock = new Mock<IProductService>();
            _orderServiceMock = new Mock<IOrderService>();
            _target = new OrderDetailController(_categoryServiceMock.Object, _cacheManagerMock.Object, _orderServiceMock.Object, _productServiceMock.Object);
        }

        [TestMethod]
        [TestCategory("OrderDetailControllerTests")]
        public async Task DeleteReturnsRedirectToAction()
        {
            _orderServiceMock.Setup(m => m.GetItemAsync(It.IsAny<int>())).ReturnsAsync(new Order());

            var result = await _target.Delete(It.IsAny<int>(), It.IsAny<int>());

            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }

        [TestMethod]
        [TestCategory("OrderDetailControllerTests")]
        public async Task DeleteReturnsRedirectWithParameterId()
        {
            var orderId = 1;
            _orderServiceMock.Setup(m => m.GetItemAsync(It.IsAny<int>())).ReturnsAsync(new Order() { OrderID = 1 });

            var result = await _target.Delete(It.IsAny<int>(), orderId);

            Assert.AreEqual(orderId, ((RedirectToRouteResult)result).RouteValues["id"]);
        }
    }
}
