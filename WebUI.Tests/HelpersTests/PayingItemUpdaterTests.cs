using System;
using System.Collections.Generic;
using System.Linq;
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
            var payingItemViewModel = CreatePayingItemViewModelWithCheckedProductsInItem();

            var result = await target.UpdatePayingItemFromViewModel(payingItemViewModel);

            Assert.AreEqual(300, result.Summ);
        }

        [TestMethod]
        [TestCategory("PayingItemUpdaterTests")]
        public async Task SetCorrectSumFromViewModelIfProductsInItemAndNotInItemAreNotNull()
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
                },
                ProductsInItem = new List<Product>()
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
                        ProductID = 3,
                        ProductName = "P3",
                        Price = 100
                    }
                },
                ProductsNotInItem = new List<Product>()
                {
                    new Product()
                    {
                        ProductID = 4,
                        ProductName = "P4",
                        Price = 100
                    },
                    new Product()
                    {
                        ProductID = 5,
                        ProductName = "P5",
                        Price = 100
                    },
                    new Product()
                    {
                        ProductID = 0,
                        ProductName = "P0",
                        Price = 100
                    }
                }
            };

            var result = await target.UpdatePayingItemFromViewModel(payingItemViewModel);

            Assert.AreEqual(5, result.PayingItemProducts.Count);
            Assert.AreEqual(500, result.Summ);
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
            var payingItemViewModel = CreatePayingItemViewModelWithCheckedProductsInItem();

            var result = await target.UpdatePayingItemFromViewModel(payingItemViewModel);

            Assert.AreEqual("P1, P2, P3", result.Comment);
        }

        [TestMethod]
        [TestCategory("PayingItemUpdaterTests")]
        public async Task SetCorrectCommentFromViewModelIfProductsInItemAndNotInItemAreNotNull()
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
                },
                ProductsInItem = new List<Product>()
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
                        ProductID = 3,
                        ProductName = "P3",
                        Price = 100
                    }
                },
                ProductsNotInItem = new List<Product>()
                {
                    new Product()
                    {
                        ProductID = 4,
                        ProductName = "P4",
                        Price = 100
                    },
                    new Product()
                    {
                        ProductID = 5,
                        ProductName = "P5",
                        Price = 100
                    },
                    new Product()
                    {
                        ProductID = 0,
                        ProductName = "P0",
                        Price = 100
                    }
                }
            };

            var result = await target.UpdatePayingItemFromViewModel(payingItemViewModel);

            Assert.AreEqual(5, result.PayingItemProducts.Count);
            Assert.AreEqual("P1, P2, P3, P4, P5", result.Comment);
        }

        [TestMethod]
        [TestCategory("PayingItemUpdaterTests")]
        public async Task CreateNewPayingItemProductsFromPayingItemEditViewModel()
        {
            var payingItem = new PayingItem()
            {
                ItemID = 1,
                PayingItemProducts = new List<PayingItemProduct>()
                {
                    new PayingItemProduct()
                    {
                        Id = 1,
                        ProductId = 2,
                        PayingItemId = 1
                    },
                    new PayingItemProduct()
                    {
                        Id = 2,
                        ProductId = 3,
                        PayingItemId = 1
                    },
                }
            };
            _payinItemServiceMock.Setup(x => x.GetItemAsync(It.IsAny<int>())).ReturnsAsync(payingItem);
            var target = new PayingItemUpdater(_payinItemServiceMock.Object);
            var payingItemViewModel = CreatePayingItemViewModelWithCheckedProductsInItem();

            var result = await target.UpdatePayingItemFromViewModel(payingItemViewModel);
            var payingItemProducts = result.PayingItemProducts.ToList();

            Assert.AreEqual(3, result.PayingItemProducts.Count);
            Assert.AreEqual(1, payingItemProducts[0].ProductId);
            Assert.AreEqual(2, payingItemProducts[1].ProductId);
            Assert.AreEqual(3, payingItemProducts[2].ProductId);
        }

        [TestMethod]
        [TestCategory("PayingItemUpdaterTests")]
        public async Task SetCorrectSumIfProductsInItemAreUncheckedAndProductsNotInItemAreUnchecked()
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
                },
                ProductsInItem = new List<Product>()
                {
                    new Product()
                    {
                        ProductID = 0,
                        Price = 200
                    },
                    new Product()
                    {
                        ProductID = 0,
                        Price = 100
                    }
                },
                ProductsNotInItem = new List<Product>()
                {
                    new Product()
                    {
                        ProductID = 0,
                        Price = 100
                    },
                    new Product()
                    {
                        ProductID = 0,
                        Price = 200
                    }
                }
            };

            var result = await target.UpdatePayingItemFromViewModel(payingItemViewModel);

            Assert.AreEqual(0, result.Summ);
        }

        [TestMethod]
        [TestCategory("PayingItemUpdaterTests")]
        public async Task SetCorrectSumIfProductsInItemAreUncheckedAndProductsNotInItemAreNull()
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
                },
                ProductsInItem = new List<Product>()
                {
                    new Product()
                    {
                        ProductID = 0,
                        Price = 200
                    },
                    new Product()
                    {
                        ProductID = 0,
                        Price = 100
                    }
                }                
            };

            var result = await target.UpdatePayingItemFromViewModel(payingItemViewModel);

            Assert.AreEqual(500, result.Summ);
        }

        private PayingItemEditViewModel CreatePayingItemViewModelWithCheckedProductsInItem()
        {
            return new PayingItemEditViewModel()
            {
                PayingItem = new PayingItem()
                {
                    ItemID = 1,
                    Summ = 500M,
                    Comment = "CommentFromViewModel"
                },
                ProductsInItem = new List<Product>()
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
                        ProductID = 3,
                        ProductName = "P3",
                        Price = 100
                    }
                }
            };
        }
    }
}
