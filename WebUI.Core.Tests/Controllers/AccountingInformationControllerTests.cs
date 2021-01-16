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
using WebUI.Controllers;
using WebUI.Core.Abstract;
using WebUI.Core.Exceptions;
using WebUI.Core.Models;
using WebUI.Core.Models.BudgetModels;

namespace WebUI.Tests.ControllersTests
{
    [TestClass]
    public class AccountingInformationControllerTests
    {
        private readonly Mock<IAccountService> _accountServiceMock;
        private readonly Mock<IPayingItemService> _payingItemsServiceMock;
        private readonly Mock<IReportHelper> _reportHelper;

        private List<Account> Accounts => new List<Account>()
            {
                new Account() {AccountID = 1,AccountName = "Acc 1", Cash = 100M, UserId = "1"},
                new Account() {AccountID = 2,AccountName = "Acc 2", Cash = 200M, UserId = "1"},
                new Account() {AccountID = 3,AccountName = "Acc 3", Cash = 1000M, UserId = "1"},
            };

        public AccountingInformationControllerTests()
        {
            _accountServiceMock = new Mock<IAccountService>();
            _payingItemsServiceMock = new Mock<IPayingItemService>();
            _reportHelper = new Mock<IReportHelper>();
        }

        [TestMethod]
        [TestCategory("AccountingInformationControllerTests")]
        public async Task Accounts_ReturnPartialViewWithAccountsForUser()
        {
            var userId = "1";
            var account = new Account() { UserId = userId };
            _accountServiceMock.Setup(m => m.GetListAsync(It.Is<Expression<Func<Account, bool>>>(x => x.Compile()(account)))).ReturnsAsync(Accounts.Where(x => x.UserId == userId));
            var target = new AccountingInformationController(_accountServiceMock.Object, null, null);

            var result = await target.Accounts(new WebUser() { Id = "1" });
            var model = (((PartialViewResult)result).Model as IEnumerable<Account>)?.ToList();

            Assert.IsNotNull(model);
            Assert.AreEqual(3, model?.Count);
            Assert.AreEqual(1, model?[0].AccountID);
            Assert.AreEqual(2, model?[1].AccountID);
            Assert.AreEqual(3, model?[2].AccountID);
        }

        [TestMethod]
        [TestCategory("AccountingInformationControllerTests")]
        [ExpectedException(typeof(WebUiException))]
        public async Task Accounts_RaisesWebUiException()
        {
            _accountServiceMock.Setup(m => m.GetListAsync(It.IsAny<Expression<Func<Account, bool>>>())).Throws<ServiceException>();
            var target = new AccountingInformationController(_accountServiceMock.Object, null, null);

            await target.Accounts(new WebUser());
        }

        [TestMethod]
        [TestCategory("AccountingInformationControllerTests")]
        public async Task Incomes_ReturnsPartialWithIncomesForDayWeekMonth()
        {
            _payingItemsServiceMock.Setup(m => m.GetListByTypeOfFlowAsync(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(new List<PayingItem>());
            _reportHelper.Setup(m => m.GetSummForDay(It.IsAny<List<PayingItem>>())).Returns(500);
            _reportHelper.Setup(m => m.GetSummForMonth(It.IsAny<List<PayingItem>>())).Returns(300);
            _reportHelper.Setup(m => m.GetSummForWeek(It.IsAny<List<PayingItem>>())).Returns(1000);
            var target = new AccountingInformationController(null, _reportHelper.Object, _payingItemsServiceMock.Object);

            var result = await target.Incomes(new WebUser());
            var model = ((PartialViewResult)result).Model as BudgetModel;

            Assert.AreEqual(500, model.Day);
            Assert.AreEqual(300, model.Month);
            Assert.AreEqual(1000, model.Week);
        }

        [TestMethod]
        [TestCategory("AccountingInformationControllerTests")]
        public async Task Outgoes_ReturnPartialViewWithOutgoesForDayWeekMonth()
        {
            _payingItemsServiceMock.Setup(m => m.GetListByTypeOfFlowAsync(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(new List<PayingItem>());
            var target = new AccountingInformationController(null, _reportHelper.Object, _payingItemsServiceMock.Object);
            _reportHelper.Setup(m => m.GetSummForDay(It.IsAny<List<PayingItem>>())).Returns(1000);
            _reportHelper.Setup(m => m.GetSummForMonth(It.IsAny<List<PayingItem>>())).Returns(1500);
            _reportHelper.Setup(m => m.GetSummForWeek(It.IsAny<List<PayingItem>>())).Returns(1300);

            var result = await target.Outgoes(new WebUser());
            var model = ((PartialViewResult)result).Model as BudgetModel;

            Assert.AreEqual(1000, model.Day);
            Assert.AreEqual(1300, model.Week);
            Assert.AreEqual(1500, model.Month);
        }

        [TestMethod]
        [TestCategory("AccountingInformationControllerTests")]
        [ExpectedException(typeof(WebUiException))]
        public async Task Incomes_RaisesWebUiException()
        {
            _payingItemsServiceMock.Setup(m => m.GetListByTypeOfFlowAsync(It.IsAny<string>(), It.IsAny<int>()))
                .Throws<ServiceException>();
            var target = new AccountingInformationController(null, null, _payingItemsServiceMock.Object);

            await target.Incomes(new WebUser());
        }

        [TestMethod]
        [TestCategory("AccountingInformationControllerTests")]
        [ExpectedException(typeof(WebUiException))]
        public async Task Outgoes_RaisesWebUiException()
        {
            _payingItemsServiceMock.Setup(m => m.GetListByTypeOfFlow(It.IsAny<IWorkingUser>(), It.IsAny<int>()))
                .Throws<ServiceException>();
            var target = new AccountingInformationController(null, null, _payingItemsServiceMock.Object);

            await target.Outgoes(new WebUser());
        }
    }
}
