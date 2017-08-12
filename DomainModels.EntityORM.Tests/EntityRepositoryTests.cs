using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DomainModels.Model;
using Moq;
using System.Data.Entity;

namespace DomainModels.EntityORM.Tests
{
    [TestClass]
    public class EntityRepositoryTests
    {
        [TestMethod]
        [TestCategory("EntityRepositoryTests")]
        public void GetListRaiseEception()
        {
            var mockContext = new Mock<AccountingContext>();
            var repo = new EntityRepository<Account>(mockContext.Object);
            //mockContext.Setup(x => x.Account).Throws(new excep)
            
        }
    }
}
