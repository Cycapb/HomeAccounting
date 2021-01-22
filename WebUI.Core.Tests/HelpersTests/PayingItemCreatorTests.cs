using DomainModels.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Services;
using Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Core.Exceptions;
using WebUI.Core.Implementations;
using WebUI.Core.Models.PayingItemModels;

namespace WebUI.Tests.HelpersTests
{
    [TestClass]
    public class PayingItemCreatorTests
    {
        private readonly Mock<IPayingItemService> _payingItemService;

        public PayingItemCreatorTests()
        {
            _payingItemService = new Mock<IPayingItemService>();
        }

        [TestMethod]
        [TestCategory("PayingItemCreatorTests")]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task CreatePayingItemFromViewModel_Throws_ArgumentNullException_IfInputModelIsNull()
        {
            var target = new PayingItemCreator(null);
            await target.CreatePayingItemFromViewModel(null);
        }

        [TestMethod]
        [TestCategory("PayingItemCreatorTests")]
        public async Task CreatePayingItemFromViewModel_ReturnsPayingItemViewModelWithForCorrectPayingItemDate()
        {
            var payingItemModel = new PayingItemModel()
            {
                PayingItem = new PayingItem()
                {
                    Date = DateTime.Today.AddMonths(1)
                },
                Products = null
            };
            var target = new PayingItemCreator(_payingItemService.Object);

            await target.CreatePayingItemFromViewModel(payingItemModel);

            Assert.AreEqual(DateTime.Today.Date, payingItemModel.PayingItem.Date);
        }

        [TestMethod]
        [TestCategory("PayingItemCreatorTests")]
        public async Task CreatePayingItemFromViewModel_ReturnsPayingItemViewModelWithCorrectSumIfProductsNotNull()
        {
            var payingItemModel = CreatePayingItemModel();
            var target = new PayingItemCreator(_payingItemService.Object);

            await target.CreatePayingItemFromViewModel(payingItemModel);

            Assert.AreEqual(payingItemModel.Products.Sum(x => x.Price), payingItemModel.PayingItem.Summ);
        }

        [TestMethod]
        [TestCategory("PayingItemCreatorTests")]
        public async Task CreatePayingItemFromViewModel_ReturnsPayingItemViewModelWithCorrectCommentIfPayingItemCommentIsEmpty()
        {
            var payingItemModel = CreatePayingItemModel();
            var target = new PayingItemCreator(_payingItemService.Object);

            await target.CreatePayingItemFromViewModel(payingItemModel);

            Assert.AreEqual("Product_1, Product_2, Product_3", payingItemModel.PayingItem.Comment);
        }

        [TestMethod]
        [TestCategory("PayingItemCreatorTests")]
        public async Task CreatePayingItemFromViewModel_ReturnsPayingItemViewModelWithCorrectCommentIfPayingItemCommentIsNotEmpty()
        {
            var payingItemModel = new PayingItemModel()
            {
                PayingItem = new PayingItem()
                {
                    Date = DateTime.Today,
                    Comment = "PayingItemComment"
                },
                Products = new List<Product>()
                {
                    new Product()
                    {
                        CategoryID = 1,
                        ProductName = "Product_1",
                        ProductID = 1,
                        Price = 100
                    },
                    new Product()
                    {
                        CategoryID = 1,
                        ProductID = 2,
                        ProductName = "Product_2",
                        Price = 100
                    },
                    new Product()
                    {
                        CategoryID = 1,
                        ProductID = 3,
                        ProductName = "Product_3",
                        Price = 100
                    }
                }
            };
            var target = new PayingItemCreator(_payingItemService.Object);

            await target.CreatePayingItemFromViewModel(payingItemModel);

            Assert.AreEqual("PayingItemComment", payingItemModel.PayingItem.Comment);
        }

        [TestMethod]
        [TestCategory("PayingItemCreatorTests")]
        public async Task CreatePayingItemFromViewModel_CheckIfPayingItemProductsAreAddedCorrectly()
        {
            var payingItemModel = new PayingItemModel()
            {
                PayingItem = new PayingItem()
                {
                    Date = DateTime.Today
                },
                Products = new List<Product>()
                {
                    new Product()
                    {
                        CategoryID = 1,
                        ProductName = "Product_1",
                        ProductID = 1,
                        Price = 100
                    },
                    new Product()
                    {
                        CategoryID = 1,
                        ProductID = 2,
                        ProductName = "Product_2",
                        Price = 100
                    },
                    new Product()
                    {
                        CategoryID = 1,
                        ProductID = 0,
                        ProductName = "Product_3",
                        Price = 100
                    }
                }
            };
            var target = new PayingItemCreator(_payingItemService.Object);

            await target.CreatePayingItemFromViewModel(payingItemModel);

            // PayingItem.PayingItemProducts  must include only those PayingTemModel.Products where ProductId != 0
            Assert.AreEqual(2, payingItemModel.PayingItem.PayingItemProducts.Count);
        }

        [TestMethod]
        [TestCategory("PayingItemCreatorTests")]
        public async Task ThrowsWebUiExceptionWithInnerServiceException()
        {
            _payingItemService.Setup(x => x.CreateAsync(It.IsAny<PayingItem>())).ThrowsAsync(new ServiceException());
            var target = new PayingItemCreator(_payingItemService.Object);
            var payingItemModel = CreatePayingItemModel();

            try
            {
                await target.CreatePayingItemFromViewModel(payingItemModel);
            }
            catch (WebUiException ex)
            {
                Assert.IsInstanceOfType(ex.InnerException, typeof(ServiceException));
            }
        }

        private PayingItemModel CreatePayingItemModel()
        {
            return new PayingItemModel()
            {
                PayingItem = new PayingItem()
                {
                    Date = DateTime.Today
                },
                Products = new List<Product>()
                {
                    new Product()
                    {
                        CategoryID = 1,
                        ProductName = "Product_1",
                        ProductID = 1,
                        Price = 100
                    },
                    new Product()
                    {
                        CategoryID = 1,
                        ProductID = 2,
                        ProductName = "Product_2",
                        Price = 100
                    },
                    new Product()
                    {
                        CategoryID = 1,
                        ProductID = 3,
                        ProductName = "Product_3",
                        Price = 100
                    }
                }
            };
        }
    }
}
