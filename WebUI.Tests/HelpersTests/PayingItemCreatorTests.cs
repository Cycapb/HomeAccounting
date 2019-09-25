using DomainModels.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Services;
using Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Helpers;
using WebUI.Models;
using WebUI.Exceptions;

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

        private TestContext _testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return _testContextInstance;
            }
            set
            {
                _testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        [TestCategory("PayingItemCreatorTests")]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task CreatePayingItem_Throws_ArgumentNullException()
        {
            var target = new PayingItemCreator(null);
            await target.CreatePayingItemFromViewModel(null);
        }

        [TestMethod]
        [TestCategory("PayingItemCreatorTests")]
        public async Task CheckForCorrectPayingItemDate()
        {
            var payingItemModel = new PayingItemViewModel()
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
        public async Task CheckPayingItemSumCorrectnessIfProductsNotNull()
        {
            var payingItemModel = CreatePayingItemModel();
            var target = new PayingItemCreator(_payingItemService.Object);

            await target.CreatePayingItemFromViewModel(payingItemModel);

            Assert.AreEqual(payingItemModel.Products.Sum(x => x.Price), payingItemModel.PayingItem.Summ);
        }

        [TestMethod]
        [TestCategory("PayingItemCreatorTests")]
        public async Task CheckCommentCorrectnessIfPayingItemCommentIsEmpty()
        {
            var payingItemModel = CreatePayingItemModel();
            var target = new PayingItemCreator(_payingItemService.Object);

            await target.CreatePayingItemFromViewModel(payingItemModel);

            Assert.AreEqual("Product_1, Product_2, Product_3", payingItemModel.PayingItem.Comment);
        }

        [TestMethod]
        [TestCategory("PayingItemCreatorTests")]
        public async Task CheckCommentCorrectnessIfPayingItemCommentIsNotEmpty()
        {
            var payingItemModel = new PayingItemViewModel()
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
        public async Task CheckIfPayingItemProductsAreAddedCorrectly()
        {
            var payingItemModel = new PayingItemViewModel()
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

        private PayingItemViewModel CreatePayingItemModel()
        {
            return new PayingItemViewModel()
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
