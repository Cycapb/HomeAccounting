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
using System.Linq.Expressions;
using System;

namespace WebUI.Tests.ControllersTests
{
    [TestClass]
    public class NavLeftControllerTests
    {
        private readonly Mock<IAccountService> _accountServiceMock;
        private List<Account> _accounts;

        public NavLeftControllerTests()
        {
            _accountServiceMock = new Mock<IAccountService>();
            _accounts = new List<Account>()
            {
                new Account() {AccountID = 1,AccountName = "Acc 1", Cash = 100M, UserId = "1"},
                new Account() {AccountID = 2,AccountName = "Acc 2", Cash = 200M, UserId = "1"},
                new Account() {AccountID = 3,AccountName = "Acc 3", Cash = 1000M, UserId = "1"},
            };
        }

        [TestMethod]
        [TestCategory("NavLeftControllerTests")]
        public void Can_Get_Accounts_By_UserId()
        {            
            var userId = "1";
            _accountServiceMock.Setup(m => m.GetList(It.IsAny<Expression<Func<Account, bool>>>())).Returns(_accounts.Where(x => x.UserId == userId));            
            var target = new NavLeftController(_accountServiceMock.Object, null);

            var result = (((PartialViewResult) target.GetAccounts(new WebUser() {Id = "1"})).Model as IEnumerable<Account>)?.ToList();

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result?.Count);
            Assert.AreEqual(1, result?[0].AccountID);
            Assert.AreEqual(2, result?[1].AccountID);
            Assert.AreEqual(3, result?[2].AccountID);
        }

        [TestMethod]
        [TestCategory("NavLeftControllerTests")]
        [ExpectedException(typeof(WebUiException))]
        public void GetAccounts_RaisesWebUiException()
        {
            _accountServiceMock.Setup(m => m.GetList(It.IsAny<Expression<Func<Account, bool>>>())).Throws<ServiceException>();
            var target = new NavLeftController(_accountServiceMock.Object, null);

            target.GetAccounts(new WebUser());
        }
    }
}
