using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using DomainModels.Exceptions;
using DomainModels.Model;
using WebUI.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Services;
using Services.Exceptions;
using WebUI.Exceptions;

namespace WebUI.Tests.ControllersTests
{
    [TestClass]
    public class DebtControllerTests
    {
        private readonly Mock<IDebtService> _debtService;
        private readonly DebtController _debtController;

        public DebtControllerTests()
        {
            _debtService = new Mock<IDebtService>();
            _debtController = new DebtController(_debtService.Object, null, null);
        }

        [TestMethod]
        [TestCategory("DebtControllerTests")]
        public async Task Delete_ReturnsRedirectToRouteResult()
        { 
            var result = await _debtController.Delete(It.IsAny<int>());
            var resultType = (result as RedirectToRouteResult);

            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            Assert.IsNotNull(resultType);
            Assert.AreEqual(resultType.RouteValues["action"], "DebtList");
        }

        [TestMethod]
        [TestCategory("DebtControllerTests")]
        public async Task Delete_DeleteAsyncExactlyOnce()
        {
            await _debtController.Delete(It.IsAny<int>());

            _debtService.Verify(m => m.DeleteAsync(It.IsAny<int>()), Times.Exactly(1));
        }

        [TestMethod]
        [TestCategory("DebtControllerTests")]
        public async Task ClosePartially_NoDebtWithSuchId_RedirectToDebtList()
        {
            _debtService.Setup(x => x.GetItemAsync(It.IsAny<int>())).ReturnsAsync((Debt)null);

            var result = await _debtController.ClosePartially(It.IsAny<int>());
            var viewResult = result as RedirectToRouteResult;

            Assert.IsNotNull(viewResult);
            Assert.AreEqual("DebtList", viewResult.RouteValues["action"]);
        }

        [TestMethod]
        [TestCategory("DebtControllerTests")]
        public async Task ClosePartially_DebtWithIdExists_ReturnPartialView_ClosePartially()
        {
            _debtService.Setup(x => x.GetItemAsync(It.IsAny<int>())).ReturnsAsync(new Debt());

            var result = await _debtController.ClosePartially(It.IsAny<int>());
            var viewResult = result as PartialViewResult;
            
            Assert.IsNotNull(viewResult);
            Assert.AreEqual("_ClosePartially", viewResult.ViewName);
        }

        [TestMethod]
        [TestCategory("DebtControllerTests")]
        [ExpectedException(typeof(WebUiException))]
        public async Task ClosePartially_ThrowsWebUiException()
        {
            _debtService.Setup(x => x.GetItemAsync(It.IsAny<int>())).Throws<ServiceException>();

            await _debtController.ClosePartially(It.IsAny<int>());
        }

        [TestMethod]
        [TestCategory("DebtControllerTests")]
        public async Task ClosePartially_ThrowsWebUiException_WithInnerServiceException()
        {
            _debtService.Setup(x => x.GetItemAsync(It.IsAny<int>())).Throws<ServiceException>();

            try
            {
                await _debtController.ClosePartially(It.IsAny<int>());
            }
            catch (WebUiException e)
            {
               Assert.IsInstanceOfType(e.InnerException, typeof(ServiceException));
            }
        }
    }
}
