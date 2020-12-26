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

using WebUI.Core.Controllers;
using WebUI.Core.Abstract;
using WebUI.Core.Exceptions;
using WebUI.Core.Models;
using WebUI.Core.Models.CategoryModels;
using WebUI.Core.Models.PayingItemModels;
using Microsoft.AspNetCore.Mvc;

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
        public async Task Add_HttpGet_ReturnsPartialView()
        {
            _categoryServiceMock.Setup(m => m.GetActiveGategoriesByUserAsync(It.IsAny<string>())).ReturnsAsync(new List<Category>());
            _accountServiceMock.Setup(m => m.GetListAsync()).ReturnsAsync(new List<Account>());
            var target = new PayingItemController(null, _categoryServiceMock.Object, _accountServiceMock.Object, null, null, null);

            var result = await target.Add(new WebUser() { Id = "1" }, 1);
            var model = ((PartialViewResult)result).Model as PayingItemModel;
            var viewDataCategories = ((PartialViewResult)result).ViewData["Categories"] as IEnumerable<Category>;
            var viewDataAccounts = ((PartialViewResult)result).ViewData["Accounts"] as IEnumerable<Account>;

            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
            Assert.AreEqual(model.Products.Count, 0);
            Assert.IsNotNull(model.PayingItem);
            Assert.AreEqual(viewDataCategories.Count(), 0);
            Assert.AreEqual(viewDataAccounts.Count(), 0);
        }

        [TestCategory("PayingItemControllerTests")]
        [TestMethod]
        public async Task Add_HttpPost_InvalidModelState_ReturnsPartialView()
        {
            _categoryServiceMock.Setup(m => m.GetActiveGategoriesByUserAsync(It.IsAny<string>())).ReturnsAsync(new List<Category>());
            _accountServiceMock.Setup(m => m.GetListAsync()).ReturnsAsync(new List<Account>());
            var target = new PayingItemController(null, _categoryServiceMock.Object, _accountServiceMock.Object, null, null, null);
            target.ModelState.AddModelError("", "");

            var result = await target.Add(new WebUser(), new PayingItemModel() { PayingItem = new PayingItem(), Products = new List<Product>() }, 1);
            var viewDataCategories = ((PartialViewResult)result).ViewData["Categories"] as IEnumerable<Category>;
            var viewDataAccounts = ((PartialViewResult)result).ViewData["Accounts"] as IEnumerable<Account>;
            var model = ((PartialViewResult)result).ViewData.Model as PayingItemModel;

            Assert.IsInstanceOfType(result, typeof(PartialViewResult));            
            Assert.IsNotNull(model.PayingItem);
        }

        [TestMethod]
        [TestCategory("PayingItemControllerTests")]
        public async Task Add_HttpPost_ValidModelState_ReturnsRedirectToActionWithNameList()
        {
            var payingItemModel = new PayingItemModel()
            {
                PayingItem = new PayingItem() { AccountID = 1, CategoryID = 1, Date = DateTime.Today, UserId = "1", ItemID = 1 },
                Products = new List<Product>()
            };
            var target = new PayingItemController(null, null, null, _payingItemCreator.Object, null, null);

            var result = await target.Add(new WebUser() { Id = "1" }, payingItemModel, It.IsAny<int>());

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            Assert.AreEqual("List", ((RedirectToActionResult)result).ActionName);
        }

        [TestMethod]
        [TestCategory("PayingItemControllerTests")]
        public async Task Add_ValidModelState_Throws_WebUiExceptionWithInnerWebUiException()
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
            var routeActionName = (result as RedirectToActionResult).ActionName;

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            Assert.AreEqual("ListAjax", routeActionName);
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
        public async Task Delete_RedirectsToActionWithNameList()
        {
            var target = new PayingItemController(_payingItemServiceMock.Object, null, null, null, null, null);

            var result = await target.Delete(new WebUser(), It.IsAny<int>());
            var redirectResult = (RedirectToActionResult)result;

            Assert.AreEqual("List", redirectResult.ActionName);
        }

        [TestMethod]
        [TestCategory("PayingItemControllerTests")]
        public async Task List_ReturnsPartialView_With_PayingItemsByDate()
        {
            var date = DateTime.Now - TimeSpan.FromDays(2);
            var payingItem = new PayingItem() { UserId = "1", Date = DateTime.Today - TimeSpan.FromDays(1) };
            var itemList = new List<PayingItem>()
                {
                    new PayingItem()
                    {
                        AccountID = 1, CategoryID = 1, Comment = "PayingItem 1", Date = DateTime.Today - TimeSpan.FromDays(4),
                        Category = new Category() {Name = "Cat1"}
                    },
                    new PayingItem()
                    {
                        AccountID = 2, CategoryID = 1, Comment = "PayingItem 2", Date = date, UserId = "1",
                        Category = new Category() {Name = "Cat2"}
                    },
                    new PayingItem()
                    {
                        AccountID = 3, CategoryID = 2, Comment = "PayingItem 3", Date = date,
                        UserId = "1", Category = new Category() { Name = "Cat3" }
                    },
                    new PayingItem()
                    {
                        AccountID = 4, CategoryID = 2, Comment = "PayingItem 4", Date = date, UserId = "1",
                        Category = new Category() { Name = "Cat4" }
                    }
                };
            _payingItemServiceMock.Setup(m => m.GetListAsync(It.Is<Expression<Func<PayingItem, bool>>>(x => x.Compile()(payingItem))))
                .ReturnsAsync(itemList.Where(i => DateTime.Now.Date - i.Date <= TimeSpan.FromDays(2) && i.UserId == "1"));
            var target = new PayingItemController(_payingItemServiceMock.Object, null, null, null, null, null);

            var result = await target.List(new WebUser() { Id = "1" });
            var viewModel = ((PartialViewResult)result).Model as PayingItemsListWithPaginationModel;

            Assert.IsTrue(viewModel.PayingItems.Count() == 3);
        }

        [TestMethod]
        [TestCategory("PayingItemControllerTests")]
        public async Task List_ItemsPerPage2_Returns_PartialViewResultWith2ItemsOnPage()
        {
            var dateMinusTwoDays = DateTime.Today - TimeSpan.FromDays(2);
            var dateMinusOneDay = DateTime.Today - TimeSpan.FromDays(1);
            var userId = "1";
            var itemList = new PayingItem[]
            {
                    new PayingItem()
                    {
                        AccountID = 1, CategoryID = 1, Comment = "PayingItem 1", Date = dateMinusTwoDays, UserId = "1",
                        Category = new Category() {Name = "Cat1"}
                    },
                    new PayingItem()
                    {
                        AccountID = 2, CategoryID = 2, Comment = "PayingItem 2", Date = dateMinusTwoDays, UserId = "1",
                        Category = new Category() { Name = "Cat2" }
                    },
                    new PayingItem()
                    {
                        AccountID = 3, CategoryID = 2, Comment = "PayingItem 3", Date = dateMinusOneDay, UserId = "1",
                        Category = new Category() { Name = "Cat3" }
                    },
                    new PayingItem()
                    {
                        AccountID = 4, CategoryID = 4, Comment = "PayingItem 4", Date = dateMinusTwoDays, UserId = "1",
                        Category = new Category() { Name = "Cat4" }
                    },
            };
            var payingItem = new PayingItem() { UserId = "1", Date = DateTime.Today - TimeSpan.FromDays(1) };
            _payingItemServiceMock.Setup(m => m.GetListAsync(It.Is<Expression<Func<PayingItem, bool>>>(x => x.Compile()(payingItem))))
                .ReturnsAsync(itemList.Where(i => i.UserId == userId && (i.Date >= dateMinusTwoDays && i.Date <= DateTime.Today)));
            var target = new PayingItemController(_payingItemServiceMock.Object, null, null, null, null, null) { ItemsPerPage = 2 };

            var result = await target.List(new WebUser() { Id = "1" }, 2);
            var viewModel = ((PartialViewResult)result).Model as PayingItemsListWithPaginationModel;

            Assert.AreEqual(2, viewModel.PayingItems.Count());
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
        public async Task Edit_HttpPost_ModelIsValid_CanUpdate_RedirectsToActionWithNameList()
        {
            var pItemEditModel = new PayingItemEditModel()
            {
                PayingItem = new PayingItem() { CategoryID = 2 }
            };
            _payingItemUpdaterMock.Setup(m => m.UpdatePayingItemFromViewModel(It.IsAny<PayingItemEditModel>())).ReturnsAsync(pItemEditModel.PayingItem);
            var target = new PayingItemController(null, null, null, null, null, _payingItemUpdaterMock.Object);

            var result = await target.Edit(new WebUser() { Id = "1" }, pItemEditModel);
            var redirectResult = (RedirectToActionResult)result;

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            Assert.AreEqual("List", redirectResult.ActionName);
        }

        [TestMethod]
        [TestCategory("PayingItemControllerTests")]
        [ExpectedException(typeof(WebUiException))]
        public async Task Edit_Get_RaisesServiceException()
        {
            _payingItemServiceMock.Setup(m => m.GetItemAsync(It.IsAny<int>())).Throws<ServiceException>();
            _categoryServiceMock.Setup(m => m.GetActiveGategoriesByUserAsync(It.IsAny<string>()))
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
    }
}