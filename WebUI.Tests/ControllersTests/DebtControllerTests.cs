using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using DomainModels.Model;
using WebUI.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Services;
using Services.Exceptions;
using WebUI.Exceptions;
using WebUI.Models.DebtViewModels;

namespace WebUI.Tests.ControllersTests
{
    [TestClass]
    public class DebtControllerTests
    {
        private readonly Mock<IDebtService> _debtService;
        private readonly Mock<ICreateCloseDebtService> _createCloseDebtService;
        private readonly DebtController _debtController;

        public DebtControllerTests()
        {
            _debtService = new Mock<IDebtService>();
            _createCloseDebtService = new Mock<ICreateCloseDebtService>();
            _debtController = new DebtController(_debtService.Object, _createCloseDebtService.Object, null);
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
        public async Task ClosePartially_Get_NoDebtWithSuchId_RedirectToDebtList()
        {
            _debtService.Setup(x => x.GetItemAsync(It.IsAny<int>())).ReturnsAsync((Debt)null);

            var result = await _debtController.ClosePartially(It.IsAny<int>());
            var viewResult = result as RedirectToRouteResult;

            Assert.IsNotNull(viewResult);
            Assert.AreEqual("DebtList", viewResult.RouteValues["action"]);
        }

        [TestMethod]
        [TestCategory("DebtControllerTests")]
        public async Task ClosePartially_Get_DebtWithIdExists_ReturnPartialView_ClosePartially()
        {
            _debtService.Setup(x => x.GetItemAsync(It.IsAny<int>())).ReturnsAsync(new Debt()
            {
                Account = new Account()
                {
                    AccountName = "TestAccount"
                }
            });

            var result = await _debtController.ClosePartially(It.IsAny<int>());
            var viewResult = result as PartialViewResult;
            
            Assert.IsNotNull(viewResult);
            Assert.AreEqual("_ClosePartially", viewResult.ViewName);
        }

        [TestMethod]
        [TestCategory("DebtControllerTests")]
        [ExpectedException(typeof(WebUiException))]
        public async Task ClosePartially_Get_ThrowsWebUiException()
        {
            _debtService.Setup(x => x.GetItemAsync(It.IsAny<int>())).Throws<ServiceException>();

            await _debtController.ClosePartially(It.IsAny<int>());
        }

        [TestMethod]
        [TestCategory("DebtControllerTests")]
        public async Task ClosePartially_Get_ThrowsWebUiException_WithInnerServiceException()
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

        [TestMethod]
        [TestCategory("DebtControllerTests")]
        public async Task ClosePartially_Post_InvalidModelState_ReturnPartialView_ClosePartially()
        {
            _debtController.ModelState.AddModelError("","");
            _debtService.Setup(x => x.GetItemAsync(It.IsAny<int>())).ReturnsAsync(new Debt());

            var result = await _debtController.ClosePartially(new DebtEditingViewModel(){ DebtId = 1 });

            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
            Assert.AreEqual("_ClosePartially", ((PartialViewResult)result).ViewName);
        }

        [TestMethod]
        [TestCategory("DebtControllerTests")]
        public async Task ClosePartially_Post_ValidModelState_ReturnRedirectToAction_DebtList()
        {
            var result = await _debtController.ClosePartially(new DebtEditingViewModel());
            var routeValues = ((RedirectToRouteResult) result).RouteValues;

            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            Assert.AreEqual("DebtList", routeValues["action"]);
        }

        [TestMethod]
        [TestCategory("DebtControllerTests")]
        public async Task ClosePartially_Post_IfSumGreaterThanDebtSum_Returns_PartialViewResult()
        {
            _createCloseDebtService.Setup(x => x.PartialCloseAsync(It.IsAny<int>(), It.IsAny<decimal>()))
                .Throws<ArgumentOutOfRangeException>();
            _debtService.Setup(x => x.GetItemAsync(It.IsAny<int>())).ReturnsAsync(new Debt());

            var result =  await _debtController.ClosePartially(new DebtEditingViewModel() { DebtId = 1 });
            
            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
        }
    }
}
