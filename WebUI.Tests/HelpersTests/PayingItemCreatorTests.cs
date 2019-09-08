using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Services;
using DomainModels.Model;
using WebUI.Helpers;
using System.Threading.Tasks;
using WebUI.Models;
using System.Linq;

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
            await target.CreatePayingItem(null);
        }

        [TestMethod]
        [TestCategory("PayingItemCreatorTests")]
        public async Task CheckForCorrectPayingItemDate()
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

            await target.CreatePayingItem(payingItemModel);

            Assert.AreEqual(DateTime.Today.Date, payingItemModel.PayingItem.Date);
        }

        [TestMethod]
        [TestCategory("PayingItemCreatorTests")]
        public async Task CheckPayingItemSumCorrectnessIfProductsNotNull()
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
                        ProductID = 1,
                        Price = 100
                    },
                    new Product()
                    {
                        CategoryID = 1,
                        ProductID = 2,
                        Price = 100
                    },
                    new Product()
                    {
                        CategoryID = 1,
                        ProductID = 3,
                        Price = 100
                    }
                }
            };
            var target = new PayingItemCreator(_payingItemService.Object);

            await target.CreatePayingItem(payingItemModel);

            Assert.AreEqual(payingItemModel.Products.Sum(x => x.Price), payingItemModel.PayingItem.Summ);
        }

        [TestMethod]
        [TestCategory("PayingItemCreatorTests")]
        public async Task CheckCommentCorrectnessIfPayingItemCommentIsEmpty()
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
                        ProductID = 3,
                        ProductName = "Product_3",
                        Price = 100
                    }
                }
            };
            var target = new PayingItemCreator(_payingItemService.Object);

            await target.CreatePayingItem(payingItemModel);

            Assert.AreEqual("Product_1, Product_2, Product_3", payingItemModel.PayingItem.Comment);
        }
    }
}
