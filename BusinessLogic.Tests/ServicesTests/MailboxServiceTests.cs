﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DomainModels.Repositories;
using DomainModels.Model;
using Moq;
using BussinnessLogic.Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using DomainModels.Exceptions;
using Services.Exceptions;

namespace BusinessLogic.Tests.ServicesTests
{
    [TestClass]
    public class MailboxServiceTests
    {
        private readonly Mock<IRepository<NotificationMailBox>> _repository;
        private readonly MailboxService _service;
        private readonly List<NotificationMailBox> _mailboxList = new List<NotificationMailBox>()
        {
            new NotificationMailBox() { Id = 1, MailBoxName = "M1"},
            new NotificationMailBox() { Id = 2, MailBoxName = "M2"}
        };

        public MailboxServiceTests()
        {
            _repository = new Mock<IRepository<NotificationMailBox>>();            
            _service = new MailboxService(_repository.Object);
        }

        [TestMethod]
        [TestCategory("MailboxServiceTests")]
        public async Task AddAsync()
        {
            var notificationMailBox = new NotificationMailBox() { Id = 1};
            _repository.Setup(m => m.Create(It.IsAny<NotificationMailBox>())).Returns(notificationMailBox);

            var result = await _service.CreateAsync(notificationMailBox);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
        }

        [TestMethod]
        [TestCategory("MailboxServiceTests")]
        [ExpectedException(typeof(ServiceException))]
        public async Task AddAsync_ThrowsServiceException()
        {
            _repository.Setup(x => x.Create(It.IsAny<NotificationMailBox>())).Throws<DomainModelsException>();
            
            await _service.CreateAsync(new NotificationMailBox());            
        }

        [TestMethod]
        [TestCategory("MailboxServiceTests")]
        public async Task DeleteAsync()
        {
            await _service.DeleteAsync(It.IsAny<int>());

            _repository.Verify(m => m.DeleteAsync(It.IsAny<int>()), Times.Exactly(1));
            _repository.Verify(m => m.SaveAsync(), Times.Exactly(1));
        }

        [TestMethod]
        [TestCategory("MailboxServiceTests")]
        public async Task GetItemAsync()
        {
            _repository.Setup(m => m.GetItemAsync(1)).ReturnsAsync(_mailboxList.Single(m => m.Id == 1));

            var result = await _service.GetItemAsync(1);

            Assert.AreEqual(result.Id, 1);
            Assert.AreEqual(result.MailBoxName, "M1");
        }

        [TestMethod]
        [TestCategory("MailboxServiceTests")]
        public async Task GetItemAsync_ReturnsNull()
        {            
            _repository.Setup(m => m.GetItemAsync(5)).ReturnsAsync(new List<NotificationMailBox>().FirstOrDefault());            

            var result = await _service.GetItemAsync(5);

            Assert.IsNull(result);
        }

        [TestMethod]
        [TestCategory("MailboxServiceTests")]
        public async Task GetListAsync()
        {
            _repository.Setup(m => m.GetListAsync()).ReturnsAsync(_mailboxList);

            var result = await _service.GetListAsync();

            Assert.IsNotNull(result);
            Assert.AreEqual(result.ToList()[0].Id, 1);
        }

    }
}
