using System.Collections.Generic;
using System.Linq;
using BussinessLogic.Services;
using HomeAccountingSystem_DAL.Model;
using HomeAccountingSystem_DAL.Repositories;
using HomeAccountingSystem_WebUI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Services.Tests
{
    [TestClass]
    public class PayingItemServiceTests
    {
        private readonly Mock<IRepository<PayingItem>> _repository;
        private readonly PayingItemService _service;

        private readonly List<PayingItem> _listOfItems = new List<PayingItem>()
        {
            new PayingItem() {UserId = "1", Category = new Category() {TypeOfFlowID = 1}},
            new PayingItem() {UserId = "1", Category = new Category() {TypeOfFlowID = 1}},
            new PayingItem() {UserId = "2", Category = new Category() {TypeOfFlowID = 2}}
        };

        public PayingItemServiceTests()
        {
            _repository = new Mock<IRepository<PayingItem>>();
            _service = new PayingItemService(_repository.Object);
        }

        [TestMethod]
        public void GetListByTypeOfFlow1()
        {
            _repository.Setup(m => m.GetList()).Returns(_listOfItems);
            

            var result = _service.GetListByTypeOfFlow(new WebUser() {Id = "1"}, 1).ToList();

            Assert.AreEqual<int>(2,result.Count);
        }

        [TestMethod]
        public void GetListByTypeOfFlowReturns0()
        {
            _repository.Setup(m => m.GetList()).Returns(_listOfItems);

            var result = _service.GetListByTypeOfFlow(new WebUser() {Id = "1"}, 3).ToList();

            Assert.AreEqual<int>(0, result.Count);
        }
    }
}
