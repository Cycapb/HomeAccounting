using BusinessLogic.Providers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessLogic.Tests.ProvidersTests
{
    [TestClass]
    public class MultipleIpAddressProviderTests
    {
        private readonly SingleIpAddressProvider _singleIpAddressProvider;        

        public MultipleIpAddressProviderTests()
        {
            _singleIpAddressProvider = new SingleIpAddressProvider();
        }

        [TestMethod]
        [TestCategory("MultipleIpAddressProviderTests")]
        public void GetIpAddresses_InputTwoAddressesOneWithPort_ReturnsTwoAddressesWithoutPort()
        {
            var multipleIpAddress = "192.168.1.1:44508,127.0.0.1";
            var target = new MultipleIpAddressProvider(new SingleIpAddressProviderLoggingDecorator(new SingleIpAddressProvider()));

            var result = target.GetIpAddresses(multipleIpAddress);

            Assert.AreEqual("192.168.1.1,127.0.0.1", result);
        }
    }
}
