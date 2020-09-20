using DomainModels.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Services;
using Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.WebPages;
using WebUI.Abstract;
using WebUI.Controllers;
using WebUI.Exceptions;
using WebUI.Models;
using WebUI.Models.CategoryModels;
using WebUI.Models.PayingItemModels;

namespace WebUI.Tests.ControllersTests
{

    [TestClass]
    public class PayingItemTest
    {
        private readonly Mock<IPayingItemService> _payingItemServiceMock;
        private readonly Mock<ICategoryService> _categoryServiceMock;
        private readonly Mock<IAccountService> _accountServiceMock;
        private readonly Mock<IPayingItemCreator> _payingItemCreator;
        private readonly Mock<IPayingItemEditViewModelCreator> _payingItemEditViewModelCreatorMock;
        private readonly Mock<IPayingItemUpdater> _payingItemUpdaterMock;

        public PayingItemTest()
        {
            _payingItemServiceMock = new Mock<IPayingItemService>();
            _categoryServiceMock = new Mock<ICategoryService>();
            _accountServiceMock = new Mock<IAccountService>();
            _payingItemCreator = new Mock<IPayingItemCreator>();
            _payingItemEditViewModelCreatorMock = new Mock<IPayingItemEditViewModelCreator>();
            _payingItemUpdaterMock = new Mock<IPayingItemUpdater>();
        }

        [TestCategory("PayingItemControllerTests")]
        [TestMethod]
        public async Task Add_ReturnPartialView()
        {
            _categoryServiceMock.Setup(m => m.GetActiveGategoriesByUser(It.IsAny<string>())).ReturnsAsync(new List<Category>());
            _accountServiceMock.Setup(m => m.GetListAsync()).ReturnsAsync(new List<Account>());
            var target = new PayingItemController(null, _categoryServiceMock.Object, _accountServiceMock.Object, null, null, null);

            var result = await target.Add(new WebUser() { Id = "1" }, 1);
            var model = ((PartialViewResult)result).ViewData.Model as PayingItemModel;
            var viewBag = ((PartialViewResult)result).ViewBag;

            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
            Assert.AreEqual(model.Products.Count, 0);
            Assert.IsNotNull(model.PayingItem);
            Assert.AreEqual(0, viewBag.Categories.Count);
            Assert.AreEqual(0, viewBag.Accounts.Count);
        }

        [TestCategory("PayingItemControllerTests")]
        [TestMethod]
        public async Task Add_InvalidModel_ReturnsPartialView()
        {
            _categoryServiceMock.Setup(m => m.GetActiveGategoriesByUser(It.IsAny<string>())).ReturnsAsync(new List<Category>());
            _accountServiceMock.Setup(m => m.GetListAsync()).ReturnsAsync(new List<Account>());
            var target = new PayingItemController(null, _categoryServiceMock.Object, _accountServiceMock.Object, null, null, null);
            target.ModelState.AddModelError("", "");

            var result = await target.Add(new WebUser(), new PayingItemModel() { PayingItem = new PayingItem(), Products = new List<Product>() }, 1);
            var viewBag = ((PartialViewResult)result).ViewBag;
            var model = ((PartialViewResult)result).ViewData.Model as PayingItemModel;

            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
            Assert.AreEqual(0, model.Products.Count);
            Assert.IsNotNull(model.PayingItem);
            Assert.AreEqual(0, viewBag.Categories.Count);
            Assert.AreEqual(0, viewBag.Accounts.Count);
        }

        [TestMethod]
        [TestCategory("PayingItemControllerTests")]
        public async Task Add_ValidModel_ReturnsRedirect_ToAction_List()
        {
            var payingItemModel = new PayingItemModel()
            {
                PayingItem = new PayingItem() { AccountID = 1, CategoryID = 1, Date = DateTime.Today, UserId = "1", ItemID = 1 },
                Products = new List<Product>()
            };
            var target = new PayingItemController(null, null, null, _payingItemCreator.Object, null, null);

            var result = await target.Add(new WebUser() { Id = "1" }, payingItemModel, It.IsAny<int>());

            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            Assert.AreEqual("List", ((RedirectToRouteResult)result).RouteValues["action"]);
        }

        [TestMethod]
        [TestCategory("PayingItemControllerTests")]
        public async Task Add_ValidModel_Throws_WebUiExceptionWithInnerWebUiException()
        {
            _payingItemCreator.Setup(m => m.CreatePayingItemFromViewModel(It.IsAny<PayingItemModel>())).ThrowsAsync(new WebUiException());
            var target = new PayingItemController(null, null, null, _payingItemCreator.Object, null, null);
            var user = new WebUser();
            var payingItemModel = new PayingItemModel()
            {
                PayingItem = new PayingItem(),
                Products = new List<Product>()
            };

            try
            {
                await target.Add(user, payingItemModel, It.IsAny<int>());
            }
            catch (WebUiException e)
            {
                Assert.IsInstanceOfType(e.InnerException, typeof(WebUiException));
            }

        }

        [TestMethod]
        [TestCategory("PayingItemControllerTests")]
        public async Task Edit_CannotGetPayingItemEditViewModel_ReturnsRedirectToListAjax()
        {
            PayingItemEditModel viewModel = null;
            _payingItemEditViewModelCreatorMock.Setup(x => x.CreateViewModel(It.IsAny<int>())).ReturnsAsync(viewModel);
            var target = new PayingItemController(_payingItemServiceMock.Object, _categoryServiceMock.Object, _accountServiceMock.Object, null, _payingItemEditViewModelCreatorMock.Object, null);

            var result = await target.Edit(new WebUser() { Id = "1" }, 1, 5);
            var routes = (result as RedirectToRouteResult).RouteValues;

            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            Assert.AreEqual(routes["action"], "ListAjax");
        }

        [TestMethod]
        [TestCategory("PayingItemControllerTests")]
        public async Task Edit_GetPayingItemEditViewModel_ReturnsPartialViewResult()
        {
            var viewModel = new PayingItemEditModel();
            _payingItemEditViewModelCreatorMock.Setup(x => x.CreateViewModel(It.IsAny<int>())).ReturnsAsync(viewModel);
            _accountServiceMock.Setup(x => x.GetList()).Returns(new List<Account>());
            _categoryServiceMock.Setup(x => x.GetList()).Returns(new List<Category>());
            _categoryServiceMock.Setup(x => x.GetItemAsync(It.IsAny<int>())).ReturnsAsync(new Category() { TypeOfFlowID = 1 });
            var target = new PayingItemController(
                null,
                _categoryServiceMock.Object,
                _accountServiceMock.Object,
                null,
                _payingItemEditViewModelCreatorMock.Object,
                null);

            var result = await target.Edit(new WebUser() { Id = "1" }, 1, 5);

            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
        }

        [TestMethod]
        [TestCategory("PayingItemControllerTests")]
        public async Task Delete_RedirectsToActionList()
        {
            PayingItemController target = new PayingItemController(_payingItemServiceMock.Object, null, null, null, null, null);

            var result = await target.Delete(new WebUser(), It.IsAny<int>());
            var redirect = (RedirectToRouteResult)result;

            _payingItemServiceMock.Verify(m => m.DeleteAsync(It.IsAny<int>()), Times.Exactly(1));
            Assert.AreEqual(redirect.RouteValues["action"], "List");
        }

        [TestMethod]
        [TestCategory("PayingItemControllerTests")]
        public void List_ReturnsPartialView_With_PayingItemsByDate()
        {
            DateTime date = DateTime.Now - TimeSpan.FromDays(2);
            var itemList = new List<PayingItem>()
                {
                    new PayingItem()
                    {
                        AccountID = 1,CategoryID = 1,Comment = "PayingItem 1",Date = "22.11.2015".AsDateTime(),
                        Category = new Category() {Name = "Cat1"}
                    },
                    new PayingItem()
                    {
                        AccountID = 2,CategoryID = 1,Comment = "PayingItem 2",Date = date,UserId = "1",
                        Category = new Category() {Name = "Cat2"}
                    },
                    new PayingItem()
                    {
                        AccountID = 3,CategoryID = 2,Comment = "PayingItem 3",Date = date,
                        UserId = "1",Category = new Category() {Name = "Cat3"}
                    },
                    new PayingItem()
                    {
                        AccountID = 4,CategoryID = 2,Comment = "PayingItem 4",Date = date,UserId = "1",
                        Category = new Category() {Name = "Cat4"}
                    }
                };
            _payingItemServiceMock.Setup(m => m.GetList(It.IsAny<Expression<Func<PayingItem, bool>>>()))
                .Returns(itemList.Where(i => DateTime.Now.Date - i.Date <= TimeSpan.FromDays(2) && i.UserId == "1"));
            PayingItemController target = new PayingItemController(_payingItemServiceMock.Object, null, null, null, null, null);

            var result = ((PartialViewResult)target.List(new WebUser() { Id = "1" })).Model as PayingItemsCollectionModel;

            Assert.AreEqual(true, result.PayingItems.Count() == 3);
        }

        [TestMethod]
        [TestCategory("PayingItemControllerTests")]
        public void Can_Paginate()
        {
            DateTime date = DateTime.Today.AddDays(-2);
            var itemList = new PayingItem[]
            {
                    new PayingItem()
                    {
                        AccountID = 4,CategoryID = 4,Comment = "PayingItem 4",Date = date,UserId = "1",
                        Category = new Category() {Name = "Cat1"}
                    },
                    new PayingItem()
                    {
                        AccountID = 1,CategoryID = 1,Comment = "PayingItem 1",Date = date,UserId = "1",
                        Category = new Category() {Name = "Cat2"}
                    },
                    new PayingItem()
                    {
                        AccountID = 2,CategoryID = 2,Comment = "PayingItem 2",Date = date,UserId = "1",
                        Category = new Category() {Name = "Cat3"}
                    },
                    new PayingItem()
                    {
                        AccountID = 3,CategoryID = 2,Comment = "PayingItem 3",Date = DateTime.Now - TimeSpan.FromDays(1),UserId = "1",
                        Category = new Category() {Name = "Cat4"}
                    }
            };
            _payingItemServiceMock.Setup(m => m.GetList(It.IsAny<Expression<Func<PayingItem, bool>>>())).Returns(itemList.Where(i => i.Date >= DateTime.Today.AddDays(-2) && i.UserId == "1"));
            var target = new PayingItemController(_payingItemServiceMock.Object, null, null, null, null, null) { ItemsPerPage = 2 };

            var pItemToView = ((PartialViewResult)target.List(new WebUser() { Id = "1" }, 2)).Model as PayingItemsCollectionModel;
            var result = pItemToView?.PayingItems.ToArray();

            Assert.AreEqual(2, result.Count());
            Assert.AreEqual(1, result[0].AccountID);
            Assert.AreEqual(2, result[1].AccountID);
        }

        [TestMethod]
        [TestCategory("PayingItemControllerTests")]
        public async Task Edit_Post_InvalidModel_ReturnsPartialView()
        {
            _accountServiceMock.Setup(x => x.GetList()).Returns(new List<Account>());
            _categoryServiceMock.Setup(x => x.GetList()).Returns(new List<Category>());
            _categoryServiceMock.Setup(x => x.GetItemAsync(It.IsAny<int>())).ReturnsAsync(new Category() { TypeOfFlowID = 1 });
            var target = new PayingItemController(null, _categoryServiceMock.Object, _accountServiceMock.Object, null, null, null);
            var pItemEditModel = new PayingItemEditModel() { PayingItem = new PayingItem() { } };
            target.ModelState.AddModelError("error", "error");

            var result = await target.Edit(new WebUser() { Id = "1" }, pItemEditModel);

            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
        }

        [TestMethod]
        [TestCategory("PayingItemControllerTests")]
        public async Task Edit_Post_ModelIsValid_CanUpdate_RedirectsToList()
        {
            var pItemEditModel = new PayingItemEditModel()
            {
                PayingItem = new PayingItem() { CategoryID = 2 }
            };
            _payingItemUpdaterMock.Setup(m => m.UpdatePayingItemFromViewModel(It.IsAny<PayingItemEditModel>())).ReturnsAsync(pItemEditModel.PayingItem);
            var target = new PayingItemController(null, null, null, null, null, _payingItemUpdaterMock.Object);

            var result = await target.Edit(new WebUser() { Id = "1" }, pItemEditModel);
            var routeResult = (RedirectToRouteResult)result;

            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            Assert.AreEqual(routeResult.RouteValues["action"], "List");
        }

        [TestMethod]
        [TestCategory("PayingItemControllerTests")]
        [ExpectedException(typeof(WebUiException))]
        public async Task Edit_Get_RaisesServiceException()
        {
            _payingItemServiceMock.Setup(m => m.GetItemAsync(It.IsAny<int>())).Throws<ServiceException>();
            _categoryServiceMock.Setup(m => m.GetActiveGategoriesByUser(It.IsAny<string>()))
                .ReturnsAsync(new List<Category>());
            _accountServiceMock.Setup(m => m.GetListAsync()).ReturnsAsync(new List<Account>());
            var target = new PayingItemController(_payingItemServiceMock.Object, _categoryServiceMock.Object, _accountServiceMock.Object, null, null, null);

            await target.Edit(new WebUser(), 1, It.IsAny<int>());
        }

        [TestMethod]
        [TestCategory("PayingItemControllerTests")]
        public async Task Edit_Get_RaisesWebUiExceptionWithInnerServiceException()
        {
            _payingItemEditViewModelCreatorMock.Setup(x => x.CreateViewModel(It.IsAny<int>())).Throws(new ServiceException());
            var target = new PayingItemController(
                null,
                null,
                null,
                null,
                _payingItemEditViewModelCreatorMock.Object,
                null);

            try
            {
                await target.Edit(new WebUser(), 1, It.IsAny<int>());
            }
            catch (WebUiException e)
            {
                Assert.IsInstanceOfType(e.InnerException, typeof(ServiceException));
            }
        }

        [TestMethod]
        [TestCategory("PayingItemControllerTests")]
        public void ExpensiveCategories()
        {
            _payingItemServiceMock.Setup(m => m.GetList()).Returns(new List<PayingItem>()
                {
                    new PayingItem() {UserId = "1",Category = new Category() {TypeOfFlowID = 2,Name = "Cat1"},Date = DateTime.Now, Summ = 100},
                    new PayingItem() {UserId = "1",Category = new Category() {TypeOfFlowID = 2,Name = "Cat2"},Date = DateTime.Now, Summ = 200},
                    new PayingItem() {UserId = "2",Category = new Category() {TypeOfFlowID = 2},Date = DateTime.Now},
                    new PayingItem() {UserId = "2",Category = new Category() {TypeOfFlowID = 12},Date = DateTime.Now},
                    new PayingItem() {UserId = "1",Category = new Category() {TypeOfFlowID = 12},Date = DateTime.Now},
                });
            WebUser user = new WebUser() { Id = "1" };
            PayingItemController target = new PayingItemController(_payingItemServiceMock.Object, null, null, null, null, null);

            //Act
            var result = ((PartialViewResult)target.ExpensiveCategories(user)).ViewData.Model as List<CategorySumModel>;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result[0].Category, "Cat2");
            Assert.AreEqual(result[1].Category, "Cat1");
            Assert.AreEqual(result.Count, 2);
        }
    }
}