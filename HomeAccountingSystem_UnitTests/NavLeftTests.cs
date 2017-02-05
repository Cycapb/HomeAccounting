using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DomainModels.Model;
using HomeAccountingSystem_DAL.Repositories;
using HomeAccountingSystem_WebUI.Controllers;
using HomeAccountingSystem_WebUI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace HomeAccountingSystem_UnitTests
{
    [TestClass]
    public class NavLeftTests
    {
        [TestMethod]
        public void Can_Get_Accounts()
        {
            Mock<IRepository<Account>> mock = new Mock<IRepository<Account>>();
            mock.Setup(m => m.GetList()).Returns(new List<Account>()
            {
                new Account() {AccountID = 1,AccountName = "Acc 1", Cash = 100M,UserId = "1"},
                new Account() {AccountID = 2,AccountName = "Acc 2", Cash = 200M,UserId = "1"},
                new Account() {AccountID = 3,AccountName = "Acc 3", Cash = 1000M,UserId = "1"},
            });
            NavLeftController target = new NavLeftController(mock.Object, null);

            var result = (((PartialViewResult) target.GetAccounts(new WebUser() {Id = "1"})).Model as IEnumerable<Account>).ToList();

            Assert.AreEqual(result.Count,3);
            Assert.AreEqual(result[0].AccountID,1);
            Assert.AreEqual(result[1].AccountID, 2);
            Assert.AreEqual(result[2].AccountID, 3);
        }
    }
}
