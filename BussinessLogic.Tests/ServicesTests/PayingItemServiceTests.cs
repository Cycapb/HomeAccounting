using System.Collections.Generic;
using System.Linq;
using Services.Exceptions;
using BussinessLogic.Services;
using DomainModels.Exceptions;
using DomainModels.Model;
using DomainModels.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebUI.Models;
using System.Linq.Expressions;
using System;
using Services;
using Services.Triggers;

namespace BussinessLogic.Tests.ServicesTests
{
    [TestClass]
    public class PayingItemServiceTests
    {
        private readonly Mock<IRepository<PayingItem>> _mockRepo;
        private readonly PayingItemService _service;
        private readonly Mock<ICategoryService> _mockCategoryService;
        private readonly Mock<IServiceTrigger<PayingItem>> _mockServiceTrigger;
        private readonly Mock<ITypeOfFlowService> _mockTypeOfFlowService;

        private readonly List<PayingItem> _listOfItems = new List<PayingItem>()
        {
            new PayingItem() {UserId = "1", Category = new Category() {TypeOfFlowID = 1}},
            new PayingItem() {UserId = "1", Category = new Category() {TypeOfFlowID = 1}},
            new PayingItem() {UserId = "2", Category = new Category() {TypeOfFlowID = 2}}
        };

        public PayingItemServiceTests()
        {
            _mockRepo = new Mock<IRepository<PayingItem>>();
            _mockCategoryService = new Mock<ICategoryService>();
            _mockServiceTrigger = new Mock<IServiceTrigger<PayingItem>>();
            _mockTypeOfFlowService = new Mock<ITypeOfFlowService>();
            _service = new PayingItemService(_mockRepo.Object, _mockServiceTrigger.Object, _mockCategoryService.Object, _mockTypeOfFlowService.Object);
        }

        [TestMethod]
        [TestCategory("PayingItemServiceTests")]
        [ExpectedException(typeof(ServiceException))]
        public void GetList_RaiseServiceException()
        {
            _mockRepo.Setup(x => x.GetList()).Throws<DomainModelsException>();

            _service.GetList();
        }

        [TestMethod]
        [TestCategory("PayingItemServiceTests")]
        public void GetList_ExceptionHasInnerDomainModelException()
        {
            _mockRepo.Setup(x => x.GetList()).Throws<DomainModelsException>();

            try
            {
                _service.GetList();
            }
            catch (ServiceException e)
            {
                Assert.IsInstanceOfType(e.InnerException, typeof(DomainModelsException));
            }
        }

        [TestMethod]
        [TestCategory("PayingItemServiceTests")]
        public void GetListByTypeOfFlow1()
        {
            var userId = "1";
            _mockRepo.Setup(m => m.GetList(It.IsAny<Expression<Func<PayingItem,bool>>>())).Returns(_listOfItems.Where(x => x.UserId == userId));

            var result = _service.GetListByTypeOfFlow(new WebUser() { Id = "1" }, 1).ToList();

            Assert.AreEqual<int>(2, result.Count);
        }

        [TestMethod]
        [TestCategory("PayingItemServiceTests")]
        public void GetListByTypeOfFlowReturns0()
        {
            _mockRepo.Setup(m => m.GetList()).Returns(_listOfItems);

            var result = _service.GetListByTypeOfFlow(new WebUser() { Id = "1" }, 3).ToList();

            Assert.AreEqual<int>(0, result.Count);
        }
    }
}
