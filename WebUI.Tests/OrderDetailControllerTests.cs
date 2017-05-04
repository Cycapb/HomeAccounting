using System.Threading.Tasks;
using System.Web.Mvc;
using DomainModels.Model;
using HomeAccountingSystem_WebUI.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Services;

namespace WebUI.Tests
{
    [TestClass]
    public class OrderDetailControllerTests
    {
        private readonly Mock<IOrderDetailService> _orderDetailService;
        private readonly OrderDetailController _target;
        private readonly Mock<ICategoryService> _categoryService;

        public OrderDetailControllerTests()
        {
            _orderDetailService = new Mock<IOrderDetailService>();
            _categoryService = new Mock<ICategoryService>();
            _target = new OrderDetailController(_orderDetailService.Object, _categoryService.Object);
        }

        [TestMethod]
        [TestCategory("OrderDetailControllerTests")]
        public async Task DeleteReturnsRedirectToAction()
        {
            _orderDetailService.Setup(m => m.GetItemAsync(It.IsAny<int>())).ReturnsAsync(new OrderDetail());

            var result = await _target.Delete(It.IsAny<int>());
            
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }

        [TestMethod]
        [TestCategory("OrderDetailControllerTests")]
        public async Task Delete()
        {
            _orderDetailService.Setup(m => m.GetItemAsync(It.IsAny<int>())).ReturnsAsync(new OrderDetail());

            await _target.Delete(It.IsAny<int>());

            _orderDetailService.Verify(m => m.DeleteAsync(It.IsAny<int>()), Times.Exactly(1));
        }

        [TestMethod]
        [TestCategory("OrderDetailControllerTests")]
        public async Task DeleteReturnsRedirectWithParameterId()
        {
            _orderDetailService.Setup(m => m.GetItemAsync(It.IsAny<int>())).ReturnsAsync(new OrderDetail() {OrderId = 1});

            var result = await _target.Delete(It.IsAny<int>());
            
            Assert.AreEqual(((RedirectToRouteResult)(result)).RouteValues["id"],1);   
        }
    }
}
