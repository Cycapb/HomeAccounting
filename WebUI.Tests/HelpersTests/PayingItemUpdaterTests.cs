using System;
using System.Threading.Tasks;
using DomainModels.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Services;
using WebUI.Helpers;
using WebUI.Models.PayingItemViewModels;

namespace WebUI.Tests.HelpersTests
{
    [TestClass]
    public class PayingItemUpdaterTests
    {
        private readonly Mock<IPayingItemService> _payinItemServiceMock;

        public PayingItemUpdaterTests()
        {
            _payinItemServiceMock = new Mock<IPayingItemService>();
        }

        [TestMethod]
        [TestCategory("PayingItemUpdaterTests")]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task PayingItemEditViewModelIsNull_ThrowsArgumentNullException()
        {            
            var target = new PayingItemUpdater(_payinItemServiceMock.Object);

            await target.UpdatePayingItemFromViewModel(null);
        }

        [TestMethod]
        [TestCategory("PayingItemUpdaterTests")]
        public async Task SetCorrectSumFromViewModelIfAllProductsAreNull()
        {
            var payingItem = new PayingItem()
            {
                ItemID = 1
            };
            _payinItemServiceMock.Setup(x => x.GetItemAsync(It.IsAny<int>())).ReturnsAsync(payingItem);
            var target = new PayingItemUpdater(_payinItemServiceMock.Object);
            var payingItemViewModel = new PayingItemEditViewModel()
            {
                PayingItem = new PayingItem()
                {
                    ItemID = 1,
                    Summ = 500M
                }
            };

            var result = await target.UpdatePayingItemFromViewModel(payingItemViewModel);

            Assert.AreEqual(500, result.Summ);
        }

        [TestMethod]
        [TestCategory("PayingItemUpdaterTests")]
        public async Task SetCorrectCommentFromViewModelIfAllProductsAreNull()
        {
            var payingItem = new PayingItem()
            {
                ItemID = 1
            };
            _payinItemServiceMock.Setup(x => x.GetItemAsync(It.IsAny<int>())).ReturnsAsync(payingItem);
            var target = new PayingItemUpdater(_payinItemServiceMock.Object);
            var payingItemViewModel = new PayingItemEditViewModel()
            {
                PayingItem = new PayingItem()
                {
                    ItemID = 1,
                    Summ = 500M,
                    Comment = "CommentFromViewModel"
                }
            };

            var result = await target.UpdatePayingItemFromViewModel(payingItemViewModel);

            Assert.AreEqual("CommentFromViewModel", result.Comment);
        }

        [TestMethod]
        [TestCategory("PayingItemUpdaterTests")]
        public async Task SetCorrectSumFromViewModelIfProductsInItemAreNotNull()
        {
            var payingItem = new PayingItem()
            {
                ItemID = 1
            };
            _payinItemServiceMock.Setup(x => x.GetItemAsync(It.IsAny<int>())).ReturnsAsync(payingItem);
            var target = new PayingItemUpdater(_payinItemServiceMock.Object);
            var payingItemViewModel = CreatePayingItemViewModelWithProductsInItem();

            var result = await target.UpdatePayingItemFromViewModel(payingItemViewModel);

            Assert.AreEqual(300, result.Summ);
        }

        [TestMethod]
        [TestCategory("PayingItemUpdaterTests")]
        public async Task SetCorrectCommentFromViewModelIfProductsInItemAreNotNull()
        {
            var payingItem = new PayingItem()
            {
                ItemID = 1
            };
            _payinItemServiceMock.Setup(x => x.GetItemAsync(It.IsAny<int>())).ReturnsAsync(payingItem);
            var target = new PayingItemUpdater(_payinItemServiceMock.Object);
            var payingItemViewModel = CreatePayingItemViewModelWithProductsInItem();

            var result = await target.UpdatePayingItemFromViewModel(payingItemViewModel);

            Assert.AreEqual("P1, P2, P3", result.Comment);
        }

        private PayingItemEditViewModel CreatePayingItemViewModelWithProductsInItem()
        {
            return new PayingItemEditViewModel()
            {
                PayingItem = new PayingItem()
                {
                    ItemID = 1,
                    Summ = 500M,
                    Comment = "CommentFromViewModel"
                },
                ProductsInItem = new System.Collections.Generic.List<Product>()
                {
                    new Product()
                    {
                        ProductID = 1,
                        ProductName = "P1",
                        Price = 100
                    },
                    new Product()
                    {
                        ProductID = 2,
                        ProductName = "P2",
                        Price = 100
                    },
                    new Product()
                    {
                        ProductID = 1,
                        ProductName = "P3",
                        Price = 100
                    }
                }
            };
        }
    }
}
