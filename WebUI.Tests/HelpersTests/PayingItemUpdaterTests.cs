using System;
using System.Threading.Tasks;
using DomainModels.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Services;
using WebUI.Helpers;

namespace WebUI.Tests.HelpersTests
{
    [TestClass]
    public class PayingItemUpdaterTests
    {
        private readonly Mock<IPayingItemService> _payinItemServiceMock;

        [TestMethod]
        [TestCategory("PayingItemUpdaterTests")]
        public async Task ProductsInItemAndNotInItemAreEmpty_ItemContainsItsOwnSumm()
        {
            //PayingItem payingItem = null;
            //_payinItemServiceMock.Setup(m => m.GetItemAsync(It.IsAny<int>())).ReturnsAsync(payingItem);           

        }
    }
}
