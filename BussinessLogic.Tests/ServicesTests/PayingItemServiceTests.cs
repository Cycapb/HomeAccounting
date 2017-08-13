using BussinessLogic.Exceptions;
using BussinessLogic.Services;
using DomainModels.Exceptions;
using DomainModels.Model;
using DomainModels.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BussinessLogic.Tests.ServicesTests
{
    [TestClass]
    public class PayingItemServiceTests
    {
        private readonly Mock<IRepository<PayingItem>> _mockRepo;
        private readonly PayingItemService _service;

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
    }
}
