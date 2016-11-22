using System;
using System.Threading.Tasks;
using BussinessLogic.Exceptions;
using BussinessLogic.Services;
using HomeAccountingSystem_DAL.Model;
using HomeAccountingSystem_DAL.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Services.Tests
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
        public async Task CreateOrder()
        {
            var orderDate = DateTime.Today.Date;
            var userId = "1";
            _orderRepository.Setup(m => m.CreateAsync(It.IsAny<Order>())).ReturnsAsync(new Order() {UserId = "1"});

            var result = await _orderService.CreateOrderAsync(orderDate, userId);

            Assert.AreEqual(result.UserId, "1");

        }

        [TestMethod]
        public void SendByEmail()
        {
            var orderId = 1;
            _orderRepository.Setup(m => m.GetItem(It.IsAny<int>())).Returns(new Order()
            {
                OrderID = 1
            });

            _orderService.SendByEmail(orderId);

            _emailSender.Verify(m => m.Send(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void SendByEmailCannotFindOrderById()
        {
            _orderService.SendByEmail(It.IsAny<int>());

            _emailSender.Verify(m => m.Send(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        [ExpectedException(typeof(SendEmailException))]
        public void SendByEmailThrowsSendEmailexceptionIfCannotSend()
        {
            _orderRepository.Setup(m => m.GetItem(It.IsAny<int>())).Returns(new Order() { OrderID = 1 });
            _emailSender.Setup(m => m.Send(It.IsAny<string>())).Throws<SendEmailException>();

            _orderService.SendByEmail(1);
        }
    }
}
    