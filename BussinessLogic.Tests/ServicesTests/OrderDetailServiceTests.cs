using System.Collections.Generic;
using System.Threading.Tasks;
using BussinessLogic.Services;
using DomainModels.Model;
using DomainModels.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BussinessLogic.Tests.ServicesTests
{
    [TestClass]
    public class OrderDetailServiceTests
    {
        private readonly OrderDetailService _orderDetailService;
        private readonly Mock<IRepository<PaiyngItemProduct>> _pItemProductRepo;
        private readonly Mock<IRepository<OrderDetail>> _orderDetailRepo;

        private readonly List<OrderDetail> _orderDetails = new List<OrderDetail>()
        {
            new OrderDetail() {ID = 1, OrderId = 2},
            new OrderDetail() {ID = 2, OrderId = 1},
            new OrderDetail() {ID = 3, OrderId = 3},
            new OrderDetail() {ID = 4, OrderId = 2}
        };

    public OrderDetailServiceTests()
        {
            _pItemProductRepo = new Mock<IRepository<PaiyngItemProduct>>();
            _orderDetailRepo = new Mock<IRepository<OrderDetail>>();
            _orderDetailService = new OrderDetailService(_orderDetailRepo.Object,_pItemProductRepo.Object);
        }

        [TestMethod]
        [TestCategory("OrderDetailServiceTests")]
        public async Task GetItemAsync()
        {
            var id = 1;
            _orderDetailRepo.Setup(m => m.GetItemAsync(1)).ReturnsAsync(_orderDetails.Find(x => x.ID == id));

            var result = await _orderDetailService.GetItemAsync(id);

            Assert.AreEqual(result.ID, 1);
            Assert.AreEqual(result.OrderId, 2);
        }
    }
}
