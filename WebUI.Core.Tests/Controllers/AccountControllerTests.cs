using DomainModels.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Services;
using Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebUI.Core.Controllers;
using WebUI.Core.Exceptions;
using WebUI.Core.Models;

namespace WebUI.Core.Tests.Controllers
{
    [TestClass]
    public class AccountControllerTests
    {
        private readonly List<Account> _accounts = new List<Account>()
        {
            new Account() {AccountID = 1, UserId = "1"},
            new Account() {AccountID = 2, UserId = "2"},
            new Account() {AccountID = 3, UserId = "1"}
        };

        private readonly Mock<IAccountService> _mockAccountService;
        private readonly AccountController _target;

        public AccountControllerTests()
        {
            _mockAccountService = new Mock<IAccountService>();
            _target = new AccountController(_mockAccountService.Object);
        }

        [TestMethod]
        [TestCategory("AccountControllerTests")]
        public async Task Index_Returns_PartialViewWithAccountsOfUser()
        {
            var userId = "1";
            var account = new Account() { UserId = userId };
            _mockAccountService.Setup(m => m.GetListAsync(It.Is<Expression<Func<Account, bool>>>(x => x.Compile()(account)))).ReturnsAsync(_accounts.Where(x => x.UserId == userId));

            var result = await _target.Index(new WebUser() { Id = "1" });
            var model = ((PartialViewResult)result).ViewData.Model as List<Account>;

            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
            Assert.IsNotNull(model);
            Assert.AreEqual(model.Count, 2);
            Assert.AreEqual(model[0].AccountID, 1);
            Assert.AreEqual(model[1].AccountID, 3);
        }

        [TestMethod]
        [TestCategory("AccountControllerTests")]
        [ExpectedException(typeof(WebUiException))]
        public async Task Index_RaiseWebUiExceptionIfException()
        {
            _mockAccountService.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<Account, bool>>>())).Throws<Exception>();

            await _target.Index(new WebUser());
        }

        [TestMethod]
        [TestCategory("AccountControllerTests")]
        [ExpectedException(typeof(WebUiException))]
        public async Task Index_RaiseWebUiExceptionIfServiceException()
        {
            _mockAccountService.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<Account, bool>>>())).Throws<ServiceException>();

            await _target.Index(new WebUser());
        }

        [TestMethod]
        [TestCategory("AccountControllerTests")]
        public async Task Index_RaiseWebUiExceptionWithInnerServiceException()
        {
            _mockAccountService.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<Account, bool>>>())).Throws<ServiceException>();
            WebUiException exception = null;

            try
            {
                await _target.Index(new WebUser());
            }
            catch (WebUiException e)
            {
                exception = e;
            }

            Assert.IsInstanceOfType(exception.InnerException, typeof(ServiceException));
        }

        [TestMethod]
        [TestCategory("AccountControllerTests")]
        [ExpectedException(typeof(WebUiException))]
        public async Task Edit_InputWebUser_RaiseWebUiException()
        {
            _mockAccountService.Setup(x => x.GetItemAsync(It.IsAny<int>())).Throws<ServiceException>();

            await _target.Edit(new WebUser(), 1);
        }

        [TestMethod]
        [TestCategory("AccountControllerTests")]
        public async Task Edit_InputWebUser_RaiseWebuiExceptionWithInnerServiceException()
        {
            WebUiException exception = null;
            _mockAccountService.Setup(x => x.GetItemAsync(It.IsAny<int>())).Throws<ServiceException>();

            try
            {
                await _target.Edit(new WebUser(), 1);
            }
            catch (WebUiException e)
            {
                exception = e;
            }

            Assert.IsInstanceOfType(exception.InnerException, typeof(ServiceException));
        }

        [TestMethod]
        [TestCategory("AccountControllerTests")]
        [ExpectedException(typeof(WebUiException))]
        public async Task Edit_InputAccount_RaiseWebUiException()
        {
            _mockAccountService.Setup(x => x.UpdateAsync(It.IsAny<Account>())).Throws<ServiceException>();

            await _target.Edit(new Account() { AccountID = 1 });
        }

        [TestMethod]
        [TestCategory("AccountControllerTests")]
        [ExpectedException(typeof(WebUiException))]
        public async Task Add_InputAccount_RaiseWebUiException()
        {
            _mockAccountService.Setup(x => x.CreateAsync(It.IsAny<Account>())).Throws<ServiceException>();

            await _target.Add(new WebUser(), new AccountAddModel());
        }

        [TestMethod]
        [TestCategory("AccountControllerTests")]
        public async Task Add_InputAccount_RaiseWebUiExceptionWithInnerServiceException()
        {
            _mockAccountService.Setup(x => x.CreateAsync(It.IsAny<Account>())).Throws<ServiceException>();
            WebUiException exception = null;

            try
            {
                await _target.Add(new WebUser(), new AccountAddModel());
            }
            catch (WebUiException e)
            {
                exception = e;
            }

            Assert.IsInstanceOfType(exception.InnerException, typeof(ServiceException));
        }

        [TestMethod]
        [TestCategory("AccountControllerTests")]
        [ExpectedException(typeof(WebUiException))]
        public async Task Delete_InputAnyAccountId_RaiseWebUiException()
        {
            _mockAccountService.Setup(x => x.DeleteAsync(It.IsAny<int>())).Throws<ServiceException>();

            await _target.Delete(1);
        }

        [TestMethod]
        [TestCategory("AccountControllerTests")]
        public async Task Delete_InputAnyAccountId_RaiseWebUiExceptionWithInnerServiceException()
        {
            _mockAccountService.Setup(x => x.DeleteAsync(It.IsAny<int>())).Throws<ServiceException>();

            try
            {
                await _target.Delete(1);
            }
            catch (WebUiException e)
            {
                Assert.IsInstanceOfType(e.InnerException, typeof(ServiceException));
            }
        }

        [TestCategory("AccountControllerTests")]
        [TestMethod]
        [ExpectedException(typeof(WebUiException))]
        public async Task TransferMoney_InputWebUser_RaiseWebUiException()
        {
            _mockAccountService.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<Account, bool>>>())).Throws<ServiceException>();

            await _target.TransferMoney(new WebUser());
        }

        [TestCategory("AccountControllerTests")]
        [TestMethod]
        public async Task TransferMoney_InputWebUser_RaiseWebUiExceptionWithInnerServiceException()
        {
            _mockAccountService.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<Account, bool>>>())).Throws<ServiceException>();
            WebUiException exception = null;

            try
            {
                await _target.TransferMoney(new WebUser());
            }
            catch (WebUiException e)
            {
                exception = e;
            }

            Assert.IsInstanceOfType(exception.InnerException, typeof(ServiceException));
        }

        [TestMethod]
        [TestCategory("AccountControllerTests")]
        [ExpectedException(typeof(WebUiException))]
        public async Task TransferMoney_InputTransferModel_RaiseWebUiException()
        {
            _mockAccountService.Setup(x => x.GetItemAsync(It.IsAny<int>())).Throws<Exception>();

            await _target.TransferMoney(new WebUser(), new TransferModel());
        }

        [TestMethod]
        [TestCategory("AccountControllerTests")]
        public async Task TransferMoney_InputTransferModel_RaiseWebUiExceptionWithInnerServiceException()
        {
            _mockAccountService.Setup(x => x.GetItemAsync(It.IsAny<int>())).Throws<ServiceException>();
            Exception exception = null;

            try
            {
                await _target.TransferMoney(new WebUser(), new TransferModel());
            }
            catch (Exception e)
            {
                exception = e;
            }

            Assert.IsInstanceOfType(exception.InnerException, typeof(ServiceException));
        }

        [TestMethod]
        [TestCategory("AccountControllerTests")]
        [ExpectedException(typeof(WebUiException))]
        public async Task GetUserAccounts_RaiseWebUiException()
        {
            _mockAccountService.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<Account, bool>>>())).ThrowsAsync(new ServiceException());

            await _target.GetUserAccounts(1, new WebUser());
        }

        [TestMethod]
        [TestCategory("AccountControllerTests")]
        public async Task GetItems_RaiseWebUiExceptionWithInnerServiceException()
        {
            _mockAccountService.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<Account, bool>>>())).ThrowsAsync(new ServiceException());
            WebUiException exception = null;

            try
            {
                await _target.GetUserAccounts(1, new WebUser());
            }
            catch (WebUiException e)
            {
                exception = e;
            }

            Assert.IsInstanceOfType(exception, typeof(WebUiException));
        }

        [TestMethod]
        [TestCategory("AccountControllerTests")]
        public async Task Edit_InputId2_Returns_PartialViewWithAccountWithId2()
        {
            _mockAccountService.Setup(m => m.GetItemAsync(It.Is<int>(v => v == 2))).ReturnsAsync(_accounts.Find(x => x.AccountID == 2));

            var result = ((PartialViewResult)await _target.Edit(new WebUser(), 2));
            var model = (Account)result.Model;

            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
            Assert.AreEqual(2, model.AccountID);
        }

        [TestMethod]
        [TestCategory("AccountControllerTests")]
        public async Task Edit_InputId0_Returns_RedirectToIndexAction()
        {
            Account acc = null;
            _mockAccountService.Setup(m => m.GetItemAsync(It.Is<int>(v => v > 3))).ReturnsAsync(acc);

            var result = (RedirectToActionResult)await _target.Edit(new WebUser(), 4);

            Assert.AreEqual(nameof(AccountController.Index), result.ActionName);
        }

        [TestMethod]
        [TestCategory("AccountControllerTests")]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task Edit_InputAccountNull_ThrowsArgumentNullException()
        {
            await _target.Edit(null);
        }

        [TestMethod]
        [TestCategory("AccountControllerTests")]
        public async Task Edit_InputModelInvalid_ReturnsPartialView()
        {
            _target.ModelState.AddModelError("", "");

            var result = await _target.Edit(new Account() { AccountID = 1 });

            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
            Assert.IsInstanceOfType(((PartialViewResult)result).Model, typeof(Account));
        }

        [TestMethod]
        [TestCategory("AccountControllerTests")]
        public async Task Edit_InputModelValid_ReturnsRedirectToIndex()
        {
            var result = await _target.Edit(new Account() { AccountID = 1 });

            _mockAccountService.Verify(m => m.UpdateAsync(It.IsAny<Account>()), Times.Once);
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
        }

        [TestMethod]
        [TestCategory("AccountControllerTests")]
        public void Add_HttpGet_ReturnsPartialView()
        {
            var result = _target.Add();
            var model = ((PartialViewResult)result).ViewData.Model as AccountAddModel;

            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
            Assert.IsNotNull(model);
        }

        [TestMethod]
        [TestCategory("AccountControllerTests")]
        public async Task Add_ModelStateValid_ReturnsRedirectToIndex()
        {
            var accountViewModel = new AccountAddModel() { AccountName = "Acc1" };

            var result = await _target.Add(new WebUser() { Id = "1" }, accountViewModel);

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
        }

        [TestMethod]
        [TestCategory("AccountControllerTests")]
        public async Task Cannot_Add_Invalid_Account()
        {
            var accountViewModel = new AccountAddModel() { AccountName = "Acc1" };
            _target.ModelState.AddModelError("error", "error");

            var result = await _target.Add(new WebUser() { Id = "1" }, accountViewModel);

            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
        }

        [TestMethod]
        [TestCategory("AccountControllerTests")]
        public async Task Delete_Input6_ReturnsRedirectToIndex()
        {
            _mockAccountService.Setup(m => m.HasAnyDependencies(It.Is<int>(v => v >= 6 && v <= 10))).Returns(false);

            var result = await _target.Delete(6);

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
        }

        [TestMethod]
        [TestCategory("AccountControllerTests")]
        public async Task Delete_Input3_ReturnsRedirectAfterDelete()
        {
            _mockAccountService.Setup(m => m.HasAnyDependencies(It.Is<int>(v => v >= 1 && v <= 5))).Returns(true);

            var result = await _target.Delete(3);

            _mockAccountService.Verify(m => m.HasAnyDependencies(3), Times.Once);
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
        }

        [TestMethod]
        [TestCategory("AccountControllerTests")]
        public async Task TransferMoney_ReturnPartialView()
        {
            var userId = "1";
            var account = new Account() { UserId = userId };
            _mockAccountService.Setup(m => m.GetListAsync(It.Is<Expression<Func<Account, bool>>>(x => x.Compile()(account)))).ReturnsAsync(_accounts.Where(x => x.UserId == userId));

            var result = ((PartialViewResult)await _target.TransferMoney(new WebUser() { Id = userId })).ViewData.Model as TransferModel;

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.FromAccounts.Count);
        }

        [TestMethod]
        [TestCategory("AccountControllerTests")]
        public async Task TransferMoneyIfAccountHasNotEnoughMoney()
        {
            _mockAccountService.Setup(m => m.GetListAsync()).ReturnsAsync(_accounts);
            _mockAccountService.Setup(m => m.GetItemAsync(It.IsAny<int>())).ReturnsAsync(new Account() { AccountID = 1, UserId = "1", Cash = 5000 });
            TransferModel tmodel = new TransferModel() { FromId = 1, ToId = 1, Summ = 1000.ToString() };
            WebUser user = new WebUser() { Id = "1" };

            var result = await _target.TransferMoney(user, tmodel);

            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
            _mockAccountService.Verify(m => m.UpdateAsync(It.IsAny<Account>()), Times.Never);
        }

        [TestMethod]
        [TestCategory("AccountControllerTests")]
        public async Task TransferMoneyModelStateInvalid()
        {
            _mockAccountService.Setup(m => m.GetListAsync()).ReturnsAsync(_accounts);
            _mockAccountService.Setup(m => m.GetItemAsync(It.IsAny<int>()))
                .ReturnsAsync(new Account() { AccountID = 1, UserId = "1", Cash = 5000 });
            TransferModel tModel = new TransferModel()
            {
                FromAccounts = ((List<Account>)await _mockAccountService.Object.GetListAsync()).ToList(),
                Summ = "1000"
            };

            _target.ModelState.AddModelError("error", "error");
            var result = await _target.TransferMoney(new WebUser() { Id = "1" }, tModel);

            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
        }

        [TestMethod]
        [TestCategory("AccountControllerTests")]
        public async Task TransferMoney_IfAccountHasEnoughMoney()
        {
            _mockAccountService.Setup(m => m.HasEnoughMoney(It.IsAny<Account>(), It.IsAny<decimal>())).Returns(true);
            _mockAccountService.Setup(m => m.GetListAsync()).ReturnsAsync(_accounts);
            _mockAccountService.Setup(m => m.GetItemAsync(It.IsAny<int>())).ReturnsAsync(new Account() { AccountID = 1, UserId = "1", Cash = 5000 });
            var tmodel = new TransferModel() { FromId = 1, ToId = 1, Summ = 1000.ToString() };
            var user = new WebUser() { Id = "1" };

            var result = await _target.TransferMoney(user, tmodel);

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            _mockAccountService.Verify(m => m.UpdateAsync(It.IsAny<Account>()), Times.AtLeastOnce);
        }

        [TestMethod]
        [TestCategory("AccountControllerTests")]        
        public async Task GetItems_Returns_PartialViewWithListofAccounts_ExceptAccountWithIdFromInput()
        {
            var exceptedAccountId = 3;
            var userId = "1";
            _mockAccountService.Setup(m => m.GetListAsync(It.IsAny<Expression<Func<Account, bool>>>())).ReturnsAsync(_accounts.Where(x => x.AccountID != exceptedAccountId && x.UserId == userId));            

            var result = (await _target.GetUserAccounts(exceptedAccountId, new WebUser() { Id = userId })).ViewData.Model as List<Account>;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 1);
            Assert.AreEqual(result[0].AccountID, 1);
        }
    }
}
