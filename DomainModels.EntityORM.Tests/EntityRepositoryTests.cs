using Microsoft.VisualStudio.TestTools.UnitTesting;
using DomainModels.Model;
using Moq;
using DomainModels.EntityORM.Exceptions;
using System.Data.Entity;

namespace DomainModels.EntityORM.Tests
{
    [TestClass]
    public class EntityRepositoryTests
    {
        [TestMethod]
        [TestCategory("EntityRepositoryTests")]
        [ExpectedException(typeof(DomainModelsException))]
        public void GetListRaiseException()
        {
            var mockContext = new Mock<AccountingContext>();
            var mockDbSet = new Mock<DbSet<Account>>();
            var repo = new EntityRepository<Account>(mockContext.Object);
            mockContext.Setup(x => x.Set<Account>()).Throws<DomainModelsException>();
            mockDbSet.Setup(x => x.Local).Throws<DomainModelsException>();
            repo.GetList();
        }
    }
}
