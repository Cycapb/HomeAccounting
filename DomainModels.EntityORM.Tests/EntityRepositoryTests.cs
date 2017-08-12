using Microsoft.VisualStudio.TestTools.UnitTesting;
using DomainModels.Model;
using Moq;
using DomainModels.EntityORM.Exceptions;

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
            var repo = new EntityRepository<Account>(mockContext.Object);
            mockContext.Setup(x => x.Account).Throws<DomainModelsException>();

            repo.GetList();
        }
    }
}
