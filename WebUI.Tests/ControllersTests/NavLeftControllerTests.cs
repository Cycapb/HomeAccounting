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
    public class NavLeftControllerTests
    {
        [TestMethod]
        [TestCategory("NavLeftControllerTests")]
        public void Can_Get_Accounts()
        {
            var mock = new Mock<IAccountService>();
            mock.Setup(m => m.GetList()).Returns(new List<Account>()
            {
                new Account() {AccountID = 1,AccountName = "Acc 1", Cash = 100M,UserId = "1"},
                new Account() {AccountID = 2,AccountName = "Acc 2", Cash = 200M,UserId = "1"},
                new Account() {AccountID = 3,AccountName = "Acc 3", Cash = 1000M,UserId = "1"},
            });
            var target = new NavLeftController(mock.Object, null);

            var result = (((PartialViewResult) target.GetAccounts(new WebUser() {Id = "1"})).Model as IEnumerable<Account>)?.ToList();

            Assert.IsNotNull(result);
            Assert.AreEqual(result?.Count,3);
            Assert.AreEqual(result?[0].AccountID,1);
            Assert.AreEqual(result?[1].AccountID, 2);
            Assert.AreEqual(result?[2].AccountID, 3);
        }

        [TestMethod]
        [TestCategory("NavLeftControllerTests")]
        [ExpectedException(typeof(WebUiException))]
        public void GetAccounts_RaisesWebUiException()
        {
            var mock = new Mock<IAccountService>();
            mock.Setup(m => m.GetList()).Throws<ServiceException>();
            var target = new NavLeftController(mock.Object, null);

            target.GetAccounts(new WebUser());
        }
    }
}
