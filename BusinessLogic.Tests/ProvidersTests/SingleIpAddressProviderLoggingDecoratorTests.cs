using BusinessLogic.Providers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Providers;
using Services.Exceptions;

namespace BusinessLogic.Tests.ProvidersTests
{
    [TestClass]
    public class SingleIpAddressProviderLoggingDecoratorTests
    {
        private readonly Mock<ISingleIpAddressProvider> _singleIpProviderMock;

        public SingleIpAddressProviderLoggingDecoratorTests()
        {
            _singleIpProviderMock = new Mock<ISingleIpAddressProvider>();
        }

        [TestMethod]
        [TestCategory("SingleIpAddressProviderLoggingDecoratorTests")]
        public void GetIpAddress_NoExceptions_ReturnsCorrectIp()
        {
            _singleIpProviderMock.Setup(m => m.GetIpAddress("192.168.1.1")).Returns("192.168.1.1");
            var target = new SingleIpAddressProviderLoggingDecorator(_singleIpProviderMock.Object);

            var result = target.GetIpAddress("192.168.1.1");

            Assert.AreEqual(result, "192.168.1.1");
        }

        [TestMethod]
        [TestCategory("SingleIpAddressProviderLoggingDecoratorTests")]
        public void GetIpAddress_ThrowsServiceException_ReturnsEmptyString()
        {
            _singleIpProviderMock.Setup(m => m.GetIpAddress(It.IsAny<string>())).Throws<ServiceException>();

            var target = new SingleIpAddressProviderLoggingDecorator(_singleIpProviderMock.Object);

            var result = target.GetIpAddress(It.IsAny<string>());

            Assert.IsTrue(string.IsNullOrEmpty(result));
        }
    }
}
