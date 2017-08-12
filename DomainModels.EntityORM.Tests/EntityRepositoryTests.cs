using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DomainModels.Model;

namespace DomainModels.EntityORM.Tests
{
    [TestClass]
    public class EntityRepositoryTests
    {
        [TestMethod]
        [TestCategory("EntityRepositoryTests")]
        public void GetListRaiseEception()
        {
            
            var repo = new EntityRepository<Account>();
            
        }
    }
}
