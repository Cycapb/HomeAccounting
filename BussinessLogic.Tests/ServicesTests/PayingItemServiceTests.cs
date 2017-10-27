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

namespace BussinessLogic.Tests.ServicesTests
{
    [TestClass]
    public class PayingItemServiceTests
    {
        private readonly Mock<IRepository<PayingItem>> _mockRepo;
        private readonly PayingItemService _service;

        private readonly List<PayingItem> _listOfItems = new List<PayingItem>()
        {
            new PayingItem() {UserId = "1", Category = new Category() {TypeOfFlowID = 1}},
            new PayingItem() {UserId = "1", Category = new Category() {TypeOfFlowID = 1}},
            new PayingItem() {UserId = "2", Category = new Category() {TypeOfFlowID = 2}}
        };

        public PayingItemServiceTests()
        {
            _mockRepo = new Mock<IRepository<PayingItem>>();
            _service = new PayingItemService(_mockRepo.Object);
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
            _mockRepo.Setup(m => m.GetList()).Returns(_listOfItems);


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
