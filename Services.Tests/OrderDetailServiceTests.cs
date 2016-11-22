using System.Collections.Generic;
using System.Threading.Tasks;
using BussinessLogic.Services;
using HomeAccountingSystem_DAL.Model;
using HomeAccountingSystem_DAL.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Services.Tests
{
    [TestClass]
    public class OrderDetailServiceTests
    {
        private readonly Mock<IRepository<OrderDetail>> _orderDetailRepository;
        private readonly OrderDetailService _orderDetailService;
        private readonly Mock<IRepository<PaiyngItemProduct>> _pItemProductRepository;

        public OrderDetailServiceTests()
        {
            _orderDetailRepository = new Mock<IRepository<OrderDetail>>();
            _pItemProductRepository = new Mock<IRepository<PaiyngItemProduct>>();
            _orderDetailService = new OrderDetailService(_orderDetailRepository.Object, _pItemProductRepository.Object);
        }

        [TestMethod]
        public async Task CreateOrderDetails()
        {
            var orderId = 1;
            var productId = 2;
            _pItemProductRepository.Setup(m => m.GetListAsync()).ReturnsAsync(new List<PaiyngItemProduct>()
            {
                new PaiyngItemProduct() {ItemID = 1,Summ = 100, ProductID = 1},
                new PaiyngItemProduct() {ItemID = 2, ProductID = 2, Summ = 200},
                new PaiyngItemProduct() {ItemID = 3, ProductID = 2,Summ = 300}
            });

            var orderDetail = await _orderDetailService.CreateOrderDetailAsync(orderId, productId);

            Assert.AreEqual(orderDetail.ProductPrice, 300);
            _orderDetailRepository.Verify(m=>m.CreateAsync(It.IsAny<OrderDetail>()), Times.Exactly(1));
        }
    }
}
