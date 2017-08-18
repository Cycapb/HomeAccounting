using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using DomainModels.Model;
using WebUI.Controllers;
using WebUI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Services;
using Services.Exceptions;
using WebUI.Exceptions;

namespace WebUI.Tests.ControllersTests
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
        public async Task IndexReturnsPartialView()
        {
            _mockAccountService.Setup(m => m.GetListAsync()).ReturnsAsync(_accounts);
            AccountController target = new AccountController(_mockAccountService.Object);

            var result = await target.Index(new WebUser() { Id = "1" });
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
        public async Task IndexRaiseWebUiException()
        {
            _mockAccountService.Setup(x => x.GetListAsync()).Throws<ServiceException>();

            var target = new AccountController(_mockAccountService.Object);

            await target.Index(new WebUser());
        }

        [TestMethod]
        [TestCategory("AccountControllerTests")]
        public async Task IndexRaiseWebUiExceptionWithInnerServiceException()
        {
            _mockAccountService.Setup(x => x.GetListAsync()).Throws<ServiceException>();

            var target = new AccountController(_mockAccountService.Object);

            try
            {
                await target.Index(new WebUser());
            }
            catch (WebUiException e)
            {
                Assert.IsInstanceOfType(e.InnerException, typeof(ServiceException));
            }
        }

        [TestMethod]
        [TestCategory("AccountControllerTests")]
        [ExpectedException(typeof(WebUiException))]
        public async Task Edit_InputWebUser_RaiseWebUiException()
        {
            _mockAccountService.Setup(x => x.GetItemAsync(It.IsAny<int>())).Throws<ServiceException>();
            var target = new AccountController(_mockAccountService.Object);

            await target.Edit(new WebUser(), 1);
        }

        [TestMethod]
        [TestCategory("AccountControllerTests")]
        public async Task Edit_InputWebUser_RaiseWebuiExceptionWithInnerServiceException()
        {
            try
            {
                await _target.Edit(new WebUser(), 1);
            }
            catch (WebUiException e)
            {
                Assert.IsInstanceOfType(e.InnerException, typeof(ServiceException));
            }
        }

        [TestMethod]
        [TestCategory("AccountControllerTests")]
        [ExpectedException(typeof(WebUiException))]
        public async Task Edit_InputAccount_RaiseWebUiException()
        {
            _mockAccountService.Setup(x => x.UpdateAsync(It.IsAny<Account>())).Throws<ServiceException>();

            await _target.Edit(new Account(){AccountID = 1});
        }

        [TestMethod]
        [TestCategory("AccountControllerTests")]
        [ExpectedException(typeof(WebUiException))]
        public async Task Add_InputAccount_RaiseWebUiException()
        {
            _mockAccountService.Setup(x => x.CreateAsync(It.IsAny<Account>())).Throws<ServiceException>();

            await _target.Add(new WebUser(), new Account());
        }

        [TestMethod]
        [TestCategory("AccountControllerTests")]
        public async Task Add_InputAccount_RaiseWebUiExceptionWithInnerServiceException()
        {
            _mockAccountService.Setup(x => x.CreateAsync(It.IsAny<Account>())).Throws<ServiceException>();

            try
            {
                await _target.Add(new WebUser(), new Account());
            }
            catch (WebUiException e)
            {
                Assert.IsInstanceOfType(e.InnerException, typeof(ServiceException));
            }
        }

        [TestMethod]
        [TestCategory("AccountControllerTests")]
        [ExpectedException(typeof(WebUiException))]
        public async Task Delete_InputInt_RaiseWebUiException()
        {
            _mockAccountService.Setup(x => x.DeleteAsync(It.IsAny<int>())).Throws<ServiceException>();

            await _target.Delete(new WebUser(), 1);
        }

        [TestMethod]
        [TestCategory("AccountControllerTests")]
        public async Task Delete_InputInt_RaiseWebUiExceptionWithInnerServiceException()
        {
            _mockAccountService.Setup(x => x.DeleteAsync(It.IsAny<int>())).Throws<ServiceException>();

            try
            {
                await _target.Delete(new WebUser(), 1);
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
            _mockAccountService.Setup(x => x.GetListAsync()).Throws<ServiceException>();

            await _target.TransferMoney(new WebUser());
        }

        [TestCategory("AccountControllerTests")]
        [TestMethod]
        public async Task TransferMoney_InputWebUser_RaiseWebUiExceptionWithInnerServiceException()
        {
            _mockAccountService.Setup(x => x.GetListAsync()).Throws<ServiceException>();

            try
            {
                await _target.TransferMoney(new WebUser());
            }
            catch (WebUiException e)
            {
                Assert.IsInstanceOfType(e.InnerException, typeof(ServiceException));
            }
        }

        [TestMethod]
        [TestCategory("AccountControllerTests")]
        [ExpectedException(typeof(WebUiException))]
        public async Task TransferMoney_InputTransferModel_RaiseWebUiException()
        {
            _mockAccountService.Setup(x => x.GetItemAsync(It.IsAny<int>())).Throws<ServiceException>();

            await _target.TransferMoney(new WebUser(), new TransferModel());
        }

        [TestMethod]
        [TestCategory("AccountControllerTests")]
        public async Task TransferMoney_InputTransferModel_RaiseWebUiExceptionWithInnerServiceException()
        {
            _mockAccountService.Setup(x => x.GetItemAsync(It.IsAny<int>())).Throws<ServiceException>();

            try
            {
                await _target.TransferMoney(new WebUser(), new TransferModel());
            }
            catch (Exception e)
            {
                Assert.IsInstanceOfType(e.InnerException, typeof(ServiceException));
            }
        }

        [TestMethod]
        [TestCategory("AccountControllerTests")]
        [ExpectedException(typeof(WebUiException))]
        public async Task GetItems_RaiseWebUiException()
        {
            _mockAccountService.Setup(x => x.GetListAsync()).Throws<ServiceException>();

            await _target.GetItems(1, new WebUser());
        }

        [TestMethod]
        [TestCategory("AccountControllerTests")]
        public async Task GetItems_RaiseWebUiExceptionWithInnerServiceException()
        {
            _mockAccountService.Setup(x => x.GetListAsync()).Throws<ServiceException>();

            try
            {
                await _target.GetItems(1, new WebUser());
            }
            catch (WebUiException e)
            {
                Assert.IsInstanceOfType(e.InnerException, typeof(ServiceException));
            }
        }

        [TestMethod]
        [TestCategory("AccountControllerTests")]
        public async Task EditInputIdReturnsNewAccount()
        {
            Account acc = null;
            _mockAccountService.Setup(m => m.GetItemAsync(It.Is<int>(v => v == 2))).ReturnsAsync(_accounts.Find(x => x.AccountID == 2));
            _mockAccountService.Setup(m => m.GetItemAsync(It.Is<int>(v => v < 1))).ReturnsAsync(acc);
            _mockAccountService.Setup(m => m.GetItemAsync(It.Is<int>(v => v > 3))).ReturnsAsync(acc);
            var target = new AccountController(_mockAccountService.Object);

            var result2 = ((PartialViewResult)await target.Edit(new WebUser(),2)).Model as Account;
            var result0 = ((PartialViewResult)await target.Edit(new WebUser(),0)).Model as Account;
            var result4 = ((PartialViewResult)await target.Edit(new WebUser(),4)).Model as Account;

            Assert.AreEqual(result0.AccountID, 0);
            Assert.AreEqual(result4.AccountID, 0);
            Assert.AreEqual(result2.AccountID,2);
        }

        [TestMethod]
        [TestCategory("AccountControllerTests")]
        public async Task EditInputAccountNullReturnsRedirectToIndex()
        {
            var target = new AccountController(null);

            var resultNull = (RedirectToRouteResult)await target.Edit(null);
            var result0 = (RedirectToRouteResult)await target.Edit(new Account() {AccountID = 0});
            
            Assert.AreEqual(resultNull.RouteValues.ContainsValue("Index"),true);
            Assert.AreEqual(result0.RouteValues.ContainsValue("Index"), true);
        }

        [TestMethod]
        [TestCategory("AccountControllerTests")]
        public async Task EditInputModelInvalidReturnsPartial()
        {
            var target = new AccountController(null);
            target.ModelState.AddModelError("","");

            var result = await target.Edit(new Account() {AccountID = 1});
            
            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
            Assert.IsInstanceOfType(((PartialViewResult)result).Model, typeof(Account));
        }

        [TestMethod]
        [TestCategory("AccountControllerTests")]
        public async Task EditInputModelValidReturnsRedirectToIndex()
        {
            
            var target = new AccountController(_mockAccountService.Object);

            var result = await target.Edit(new Account() {AccountID = 1});

            _mockAccountService.Verify(m => m.UpdateAsync(It.IsAny<Account>()),Times.Once);
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }

        [TestMethod]
        [TestCategory("AccountControllerTests")]
        public void AddReturnsPartialView()
        {
            AccountController target = new AccountController(null);

            var result = target.Add(new WebUser());
            var model = ((PartialViewResult)result).ViewData.Model as Account;

            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
            Assert.IsNotNull(model);
            Assert.AreEqual(model.AccountID, 0);
            Assert.IsNotNull(model);
        }

        [TestMethod]
        [TestCategory("AccountControllerTests")]
        public async Task AddModelStateValidReturnsRedirectToIndex()
        {
            Account account = new Account() { AccountID = 1, AccountName = "Acc1" };
            AccountController target = new AccountController(_mockAccountService.Object);

            var result = await target.Add(new WebUser() { Id = "1" }, account);

            _mockAccountService.Verify(m => m.CreateAsync(account),Times.Once);
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }

        [TestMethod]
        [TestCategory("AccountControllerTests")]
        public async Task Cannot_Add_Invalid_Account()
        {
            Account account = new Account() { AccountID = 1, AccountName = "Acc1" };
            AccountController target = new AccountController(null);
            target.ModelState.AddModelError("error", "error");

            var result = await target.Add(new WebUser() { Id = "1" }, account);

            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
        }

        [TestMethod]
        [TestCategory("AccountControllerTests")]
        public async Task DeleteInput6ReturnsRedirectToIndex()
        {
            _mockAccountService.Setup(m => m.HasAnyDependencies(It.Is<int>(v => v >= 6 && v <= 10))).Returns(false);
            var target = new AccountController(_mockAccountService.Object);

            var id6 = await target.Delete(new WebUser() {Id = "1"}, 6);

            Assert.IsInstanceOfType(id6,typeof(RedirectToRouteResult));
        }

        [TestMethod]
        [TestCategory("AccountControllerTests")]
        public async Task DeleteInput3ReturnsRedirectAfterDelete()
        {
            _mockAccountService.Setup(m => m.HasAnyDependencies(It.Is<int>(v => v >= 1 && v <= 5))).Returns(true);
            var target = new AccountController(_mockAccountService.Object);

            var id3 = await target.Delete(new WebUser() { Id = "1" }, 3);

            _mockAccountService.Verify(m => m.HasAnyDependencies(3),Times.Once);
            Assert.IsInstanceOfType(id3, typeof(RedirectToRouteResult));
        }

        [TestMethod]
        [TestCategory("AccountControllerTests")]
        public async Task TransferMoneyReturnPartialView()
        {
            _mockAccountService.Setup(m => m.GetListAsync()).ReturnsAsync(_accounts);
            AccountController target = new AccountController(_mockAccountService.Object);

            var result = ((PartialViewResult)await  target.TransferMoney(new WebUser() { Id = "1" })).ViewData.Model as TransferModel;

            Assert.AreEqual(result.FromAccounts.Count, 2);
        }

        [TestMethod]
        [TestCategory("AccountControllerTests")]
        public async Task TransferMoneyIfAccountHasNotEnoughMoney()
        {
            _mockAccountService.Setup(m => m.GetListAsync()).ReturnsAsync(_accounts);
            _mockAccountService.Setup((IAccountService m) => m.GetItemAsync(It.IsAny<int>())).ReturnsAsync(new Account() { AccountID = 1, UserId = "1", Cash = 5000 });
            TransferModel tmodel = new TransferModel() { FromId = 1, ToId = 1, Summ = 1000.ToString() };
            WebUser user = new WebUser() { Id = "1" };
            AccountController target = new AccountController(_mockAccountService.Object);

            ActionResult result = await target.TransferMoney(user, tmodel);

            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
            _mockAccountService.Verify(m => m.UpdateAsync(It.IsAny<Account>()),Times.Never);
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
            AccountController target = new AccountController(_mockAccountService.Object);

            target.ModelState.AddModelError("error", "error");
            var result = await target.TransferMoney(new WebUser() { Id = "1" }, tModel);

            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
        }

        [TestMethod]
        [TestCategory("AccountControllerTests")]
        public async Task TransferMoneyIfAccountHasEnoughMoney()
        {
            _mockAccountService.Setup(m => m.HasEnoughMoney(It.IsAny<Account>(), It.IsAny<decimal>())).Returns(true);
            _mockAccountService.Setup(m => m.GetListAsync()).ReturnsAsync(_accounts);
            _mockAccountService.Setup((IAccountService m) => m.GetItemAsync(It.IsAny<int>())).ReturnsAsync(new Account() { AccountID = 1, UserId = "1", Cash = 5000 });
            TransferModel tmodel = new TransferModel() { FromId = 1, ToId = 1, Summ = 1000.ToString() };
            WebUser user = new WebUser() { Id = "1" };
            AccountController target = new AccountController(_mockAccountService.Object);

            ActionResult result = await target.TransferMoney(user, tmodel);

            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            _mockAccountService.Verify(m => m.UpdateAsync(It.IsAny<Account>()), Times.AtLeastOnce);
        }

        [TestMethod]
        [TestCategory("AccountControllerTests")]
        public async Task GetItemsReturnsListofAccounts()
        {
            _mockAccountService.Setup(m => m.GetListAsync()).ReturnsAsync(_accounts);
            AccountController target = new AccountController(_mockAccountService.Object);
            var id = 3;

            var result = (await target.GetItems(id, new WebUser() { Id = "1" })).ViewData.Model as List<Account>;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 1);
            Assert.AreEqual(result[0].AccountID, 1);
        }
    }
}
