using System;
using System.Threading.Tasks;
using BussinessLogic.Managers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Services;
using Services.Exceptions;

namespace BussinessLogic.Tests.ManagerTests
{
    [TestClass]
    public class PlaningManagerTests
    {
        private readonly Mock<IPlanItemService> _planItemServiceMoq;

        public PlaningManagerTests()
        {
            _planItemServiceMoq = new Mock<IPlanItemService>();
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceException))]
        public async Task ClosePlaningPeriod_ThrowsServiceException()
        {
            _planItemServiceMoq.Setup(m => m.GetListAsync(It.IsAny<string>())).ThrowsAsync(new ServiceException());
            var target = new PlaningManager(_planItemServiceMoq.Object);

            await target.ClosePlaningPeriod(It.IsAny<string>(), DateTime.Today);
        }

        [TestMethod]
        public async Task ClosePlaningPeriod_ThrowsServiceException_WithInnerServiceException()
        {
            _planItemServiceMoq.Setup(m => m.GetListAsync(It.IsAny<string>())).ThrowsAsync(new ServiceException());
            var target = new PlaningManager(_planItemServiceMoq.Object);

            try
            {
                await target.ClosePlaningPeriod(It.IsAny<string>(), DateTime.Today);
            }
            catch (Exception e)
            {
                Assert.IsInstanceOfType(e.InnerException, typeof(ServiceException));
            }
        }
    }
}
