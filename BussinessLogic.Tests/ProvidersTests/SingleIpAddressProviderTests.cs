using System;
using BussinessLogic.Providers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services.Exceptions;

namespace BussinessLogic.Tests.ProvidersTests
{
    [TestClass]
    public class SingleIpAddressProviderTests
    {
        private readonly SingleIpAddressProvider _ipAddressProvider;

        public SingleIpAddressProviderTests()
        {
            _ipAddressProvider = new SingleIpAddressProvider();
        }

        [TestMethod]
        [TestCategory("SingleIpAddressProviderTests")]
        public void GetIpAddress_InputNullIpAddress_ReturnsServiceExceptionWithArgumentNullException()
        {
            try
            {
                _ipAddressProvider.GetIpAddress(null);
            }
            catch (ServiceException e)
            {
                Assert.IsInstanceOfType(e.InnerException, typeof(ArgumentNullException));    
            }
        }

        [TestMethod]
        [TestCategory("SingleIpAddressProviderTests")]
        public void GetIpAddress_InputWrongFormat_ReturnsServiceExceptionWithFormatException()
        {
            try
            {
                var wrongString = "dsdsdsd";
                _ipAddressProvider.GetIpAddress(wrongString);
            }
            catch (ServiceException e)
            {
                Assert.IsInstanceOfType(e.InnerException, typeof(FormatException));
            }
        }

        [TestMethod]
        [TestCategory("SingleIpAddressProviderTests")]
        [ExpectedException(typeof(ServiceException))]
        public void GetIpAddress_OtherExceptions_ReturnsServiceException()
        {
            var wrongAddress = "129.168.0.dfd";
            _ipAddressProvider.GetIpAddress(wrongAddress);
        }

        [TestMethod]
        [TestCategory("SingleIpAddressProviderTests")]
        public void GetIpAddress_InputIpV4_ReturnsCorrectIpV4()
        {
            var ip = $"192.168.1.25";

            var result = _ipAddressProvider.GetIpAddress(ip);

            Assert.AreEqual(result, ip);
        }

        [TestMethod]
        [TestCategory("SingleIpAddressProviderTests")]
        public void GetIpAddress_InputIpV6_ReturnsCorrectIpV6()
        {
            var ip = $"FF80:0000:0000:0000:0123:1234:ABCD:EF12";

            var result = _ipAddressProvider.GetIpAddress(ip);

            Assert.AreEqual(result, "FF80::123:1234:ABCD:EF12".ToLower());
        }
    }
}
