using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DomainModels.Repositories;
using DomainModels.Model;
using Moq;
using BussinessLogic.Services;
using System.Threading.Tasks;

namespace BussinessLogic.Tests.ServicesTests
{
    [TestClass]
    public class MailboxServiceTests
    {
        private readonly Mock<IRepository<NotificationMailBox>> _repository;
        private readonly MailboxService _service;        

        public MailboxServiceTests()
        {
            _repository = new Mock<IRepository<NotificationMailBox>>();            
            _service = new MailboxService(_repository.Object);
        }

        [TestMethod]
        public async Task AddAsync()
        {
            _repository.Setup(m => m.CreateAsync(It.IsAny<NotificationMailBox>())).ReturnsAsync(new NotificationMailBox() { Id = 1});

            var result = await _service.AddAsync(new NotificationMailBox() { Id = 1});

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
        }

        [TestMethod]
        public async Task AddAsync_ThrowsException()
        {
            _repository.Setup(m => m.CreateAsync(It.IsAny<NotificationMailBox>()))
                .ThrowsAsync<IRepository<NotificationMailBox>,NotificationMailBox>(new NotImplementedException());
            
            var result = await _service.AddAsync(new NotificationMailBox());            
        }

        [TestMethod]
        public async Task DeleteAsync()
        {
            await _service.DeleteAsync(It.IsAny<int>());

            _repository.Verify(m => m.DeleteAsync(It.IsAny<int>()), Times.Exactly(1));
            _repository.Verify(m => m.SaveAsync(), Times.Exactly(1));
        }
    }
}
