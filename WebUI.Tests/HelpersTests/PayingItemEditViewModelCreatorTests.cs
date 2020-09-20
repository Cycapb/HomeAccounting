using DomainModels.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Services;
using Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebUI.Exceptions;
using WebUI.Helpers;

namespace WebUI.Tests.HelpersTests
{
    [TestClass]
    public class PayingItemEditViewModelCreatorTests
    {
        private readonly Mock<IPayingItemService> _payingItemServiceMock;
        private readonly List<Product> _products = new List<Product>()
        {
            new Product()
            {
                ProductName = "P1",
                ProductID = 1
            },
            new Product()
            {
                ProductName = "P1",
                ProductID = 2
            },
            new Product()
            {
                ProductName = "P3",
                ProductID = 3
            },
            new Product()
            {
                ProductName = "P4",
                ProductID = 4
            }
        };

        public PayingItemEditViewModelCreatorTests()
        {
            _payingItemServiceMock = new Mock<IPayingItemService>();
        }

        [TestMethod]
        [TestCategory("PayingItemEditViewModelCreatorTests")]
        public async Task PayingItemIsNull_ReturnsNullViewModel()
        {
            PayingItem payingItem = null;
            _payingItemServiceMock.Setup(m => m.GetItemAsync(It.IsAny<int>())).ReturnsAsync(payingItem);
            var target = new PayingItemEditViewModelCreator(_payingItemServiceMock.Object);

            var result = await target.CreateViewModel(It.IsAny<int>());

            Assert.AreEqual(null, result);
        }

        [TestMethod]
        [TestCategory("PayingItemEditViewModelCreatorTests")]
        public async Task PayingItemContainsNoProducts_CategoryContainsFreeProducts()
        {
            var payingItem = new PayingItem()
            {
                Category = new Category()
                {
                    Products = new List<Product>()
                    {
                        new Product()
                        {
                            ProductName = "P1"
                        },
                        new Product()
                        {
                            ProductName = "P2"
                        }
                    }
                },
                PayingItemProducts = new List<PayingItemProduct>()
            };

            _payingItemServiceMock.Setup(m => m.GetItemAsync(It.IsAny<int>())).ReturnsAsync(payingItem);
            var target = new PayingItemEditViewModelCreator(_payingItemServiceMock.Object);

            var result = await target.CreateViewModel(1);

            Assert.AreEqual(0, result.ProductsInItem.Count);
            Assert.AreEqual(2, result.ProductsNotInItem.Count);
        }

        [TestMethod]
        [TestCategory("PayingItemEditViewModelCreatorTests")]
        public async Task PayingItemContainsProducts_CategoryContainsNoFreeProducts()
        {
            var payingItem = new PayingItem()
            {
                Category = new Category()
                {
                    Products = new List<Product>()
                },
                PayingItemProducts = new List<PayingItemProduct>()
                {
                    new PayingItemProduct()
                    {
                        Product = new Product()
                        {
                            ProductName = "P1"
                        }
                    },
                    new PayingItemProduct()
                    {
                        Product = new Product()
                        {
                            ProductName = "P2"
                        }
                    }
                }
            };

            _payingItemServiceMock.Setup(m => m.GetItemAsync(It.IsAny<int>())).ReturnsAsync(payingItem);
            var target = new PayingItemEditViewModelCreator(_payingItemServiceMock.Object);

            var result = await target.CreateViewModel(1);

            Assert.AreEqual(2, result.ProductsInItem.Count);
            Assert.AreEqual(0, result.ProductsNotInItem.Count);
        }

        [TestMethod]
        [TestCategory("PayingItemEditViewModelCreatorTests")]
        public async Task PayingItemContainsProducts_CategoryContainsFreeProducts()
        {
            var payingItem = new PayingItem()
            {
                Category = new Category()
                {
                    Products = _products
                },
                PayingItemProducts = new List<PayingItemProduct>()
                {
                    new PayingItemProduct()
                    {
                        Product = _products[0]
                    },
                    new PayingItemProduct()
                    {
                        Product = _products[1]
                    }
                }
            };

            _payingItemServiceMock.Setup(m => m.GetItemAsync(It.IsAny<int>())).ReturnsAsync(payingItem);
            var target = new PayingItemEditViewModelCreator(_payingItemServiceMock.Object);

            var result = await target.CreateViewModel(1);

            Assert.AreEqual(2, result.ProductsInItem.Count);
            Assert.AreEqual(2, result.ProductsNotInItem.Count);
        }

        [TestMethod]
        [TestCategory("PayingItemEditViewModelCreatorTests")]
        public async Task PayingItemContainsNoProducts_CategoryContainsNoProducts()
        {
            var payingItem = new PayingItem()
            {
                Category = new Category()
                {
                    Products = new List<Product>()
                },
                PayingItemProducts = new List<PayingItemProduct>()
            };

            _payingItemServiceMock.Setup(m => m.GetItemAsync(It.IsAny<int>())).ReturnsAsync(payingItem);
            var target = new PayingItemEditViewModelCreator(_payingItemServiceMock.Object);

            var result = await target.CreateViewModel(1);

            Assert.AreEqual(0, result.ProductsInItem.Count);
            Assert.AreEqual(0, result.ProductsNotInItem.Count);
        }

        [TestMethod]
        [TestCategory("PayingItemEditViewModelCreatorTests")]
        public async Task PayingItemContainsAllProductsFromCategory()
        {
            var payingItem = new PayingItem()
            {
                Category = new Category()
                {
                    Products = _products
                },
                PayingItemProducts = new List<PayingItemProduct>()
                {
                    new PayingItemProduct()
                    {
                        Product = _products[0]
                    },
                    new PayingItemProduct()
                    {
                        Product = _products[1]
                    },
                    new PayingItemProduct()
                    {
                        Product = _products[2]
                    },
                    new PayingItemProduct()
                    {
                        Product = _products[3]
                    }
                }
            };

            _payingItemServiceMock.Setup(m => m.GetItemAsync(It.IsAny<int>())).ReturnsAsync(payingItem);
            var target = new PayingItemEditViewModelCreator(_payingItemServiceMock.Object);

            var result = await target.CreateViewModel(1);

            Assert.AreEqual(4, result.ProductsInItem.Count);
            Assert.AreEqual(0, result.ProductsNotInItem.Count);
        }

        [TestMethod]
        [TestCategory("PayingItemEditViewModelCreatorTests")]
        public async Task ThrowsWebUiExceptionWithInnerServiceException()
        {
            _payingItemServiceMock.Setup(x => x.GetItemAsync(It.IsAny<int>())).Throws<ServiceException>();
            var target = new PayingItemEditViewModelCreator(_payingItemServiceMock.Object);

            try
            {
                var result = await target.CreateViewModel(It.IsAny<int>());
            }
            catch (WebUiException ex)
            {
                Assert.IsInstanceOfType(ex.InnerException, typeof(ServiceException));
            }            
        }

        [TestMethod]
        [TestCategory("PayingItemEditViewModelCreatorTests")]
        public async Task ThrowsWebUiExceptionWithInneException()
        {
            _payingItemServiceMock.Setup(x => x.GetItemAsync(It.IsAny<int>())).Throws<ServiceException>();
            var target = new PayingItemEditViewModelCreator(_payingItemServiceMock.Object);

            try
            {
                var result = await target.CreateViewModel(It.IsAny<int>());
            }
            catch (WebUiException ex)
            {
                Assert.IsInstanceOfType(ex.InnerException, typeof(Exception));
            }
        }
    }
}
