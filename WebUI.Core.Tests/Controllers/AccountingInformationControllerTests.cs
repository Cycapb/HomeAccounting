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
using WebUI.Core.Exceptions;
using WebUI.Core.Models;

namespace WebUI.Tests.ControllersTests
{
    [TestClass]
    public class AccountingInformationControllerTests
    {
        private readonly Mock<IAccountService> _accountServiceMock;
        private List<Account> _accounts => new List<Account>()
            {
                new Account() {AccountID = 1,AccountName = "Acc 1", Cash = 100M, UserId = "1"},
                new Account() {AccountID = 2,AccountName = "Acc 2", Cash = 200M, UserId = "1"},
                new Account() {AccountID = 3,AccountName = "Acc 3", Cash = 1000M, UserId = "1"},
            };

        public AccountingInformationControllerTests()
        {
            _accountServiceMock = new Mock<IAccountService>();
        }

        [TestMethod]
        [TestCategory("AccountingInformationControllerTests")]
        public async Task Can_Get_Accounts_By_UserId()
        {
            var userId = "1";
            var account = new Account() { UserId = userId };
            _accountServiceMock.Setup(m => m.GetListAsync(It.Is<Expression<Func<Account, bool>>>(x => x.Compile()(account)))).ReturnsAsync(_accounts.Where(x => x.UserId == userId));
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
    }
}
