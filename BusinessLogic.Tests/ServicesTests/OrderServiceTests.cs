﻿using System;
using System.Threading.Tasks;
using BussinnessLogic.Services;
using DomainModels.Model;
using DomainModels.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Services.Exceptions;
using Services;

namespace BusinessLogic.Tests.ServicesTests
{
    [TestClass]
    public class OrderServiceTests
    {
        private readonly OrderService _orderService;
        private readonly Mock<IRepository<Order>> _orderRepository;
        private readonly Mock<IEmailSender> _emailSender;

        public OrderServiceTests()
        {
            _orderRepository = new Mock<IRepository<Order>>();
            _emailSender = new Mock<IEmailSender>();
            _orderService = new OrderService(_orderRepository.Object, _emailSender.Object);
        }

        [TestMethod]
        [TestCategory("OrderServiceTests")]
        public async Task SendByEmail()
        {
            var orderId = 1;
            _orderRepository.Setup(m => m.GetItem(It.IsAny<int>())).Returns(new Order()
            {
                OrderID = 1
            });

            await _orderService.SendByEmailAsync(orderId, String.Empty);

            _emailSender.Verify(m => m.SendAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        [TestCategory("OrderServiceTests")]
        public async Task SendByEmailCannotFindOrderById()
        {
            await _orderService.SendByEmailAsync(It.IsAny<int>(), String.Empty);

            _emailSender.Verify(m => m.SendAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        [TestCategory("OrderServiceTests")]
        [ExpectedException(typeof(ServiceException))]
        public async Task SendByEmailThrowsServiceExceptionWithInnerSendEmailException()
        {
            _orderRepository.Setup(m => m.GetItem(It.IsAny<int>())).Returns(new Order() { OrderID = 1 });
            _emailSender.Setup(m => m.SendAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Throws<SendEmailException>();

            await _orderService.SendByEmailAsync(1, It.IsAny<string>());
        }
    }
}
    