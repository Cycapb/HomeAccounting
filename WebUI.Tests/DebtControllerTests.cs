using System.Threading.Tasks;
using System.Web.Mvc;
using HomeAccountingSystem_WebUI.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Services;

namespace WebUI.Tests
{
    [TestClass]
    public class DebtControllerTests
    {
        private readonly Mock<IDebtService> _debtService;
        private readonly Mock<IAccountService> _accService;
        private readonly DebtController _debtController;

        public DebtControllerTests()
        {
            _debtService = new Mock<IDebtService>();
            _accService = new Mock<IAccountService>();
            _debtController = new DebtController(_debtService.Object, _accService.Object);
        }

        [TestMethod]
        public async Task Delete_ReturnsRedirectToRouteResult()
        { 
            var result = await _debtController.Delete(It.IsAny<int>());
            var resultType = (result as RedirectToRouteResult);

            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            Assert.IsNotNull(resultType);
            Assert.AreEqual(resultType.RouteValues["action"], "DebtList");
        }

        [TestMethod]
        public async Task Delete_DeleteAsyncExactlyOnce()
        {
            await _debtController.Delete(It.IsAny<int>());

            _debtService.Verify(m => m.DeleteAsync(It.IsAny<int>()), Times.Exactly(1));
        }
    }
}
