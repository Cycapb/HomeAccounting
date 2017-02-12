using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DomainModels.Repositories;
using DomainModels.Model;
using Moq;
using BussinessLogic.Services;

namespace HomeAccountingSystem_UnitTests
{
    [TestClass]
    public class MailboxServiceTests
    {
        [TestMethod]
        public void Add()
        {
            var service = new MailboxService();
        }
    }
}
