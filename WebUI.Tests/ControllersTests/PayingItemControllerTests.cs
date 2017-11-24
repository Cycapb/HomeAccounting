using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.WebPages;
using DomainModels.Model;
using WebUI.Abstract;
using WebUI.Controllers;
using WebUI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Services;
using Services.Exceptions;
using WebUI.Exceptions;

namespace WebUI.Tests.ControllersTests
{

    [TestClass]
    public class PayingItemTest
    {
        private readonly Mock<IPayingItemProductHelper> _pItemProductHelper;
        private readonly Mock<IPayingItemHelper> _payingItemHelper;
        private readonly Mock<IPayingItemService> _payingItemService;
        private readonly Mock<ICategoryService> _categoryService;
        private readonly Mock<IAccountService> _accountService;

        public PayingItemTest()
        {
            _pItemProductHelper = new Mock<IPayingItemProductHelper>();
            _payingItemHelper = new Mock<IPayingItemHelper>();
            _payingItemService = new Mock<IPayingItemService>();
            _categoryService = new Mock<ICategoryService>();
            _accountService = new Mock<IAccountService>();
        }

        [TestCategory("PayingItemControllerTests")]
        [TestMethod]
        public async Task Add_ReturnPartialView()
        {
            _categoryService.Setup(m => m.GetActiveGategoriesByUser(It.IsAny<string>())).ReturnsAsync(new List<Category>());
            _accountService.Setup(m => m.GetListAsync()).ReturnsAsync(new List<Account>());
            var target = new PayingItemController(null, null, null, _categoryService.Object, _accountService.Object);

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
            _categoryService.Setup(m => m.GetActiveGategoriesByUser(It.IsAny<string>())).ReturnsAsync(new List<Category>());
            _accountService.Setup(m => m.GetListAsync()).ReturnsAsync(new List<Account>());
            var target = new PayingItemController(null, null, null, _categoryService.Object, _accountService.Object);
            target.ModelState.AddModelError("", "");

            var result = await target.Add(new WebUser(), new PayingItemModel() { PayingItem = new PayingItem(), Products = new List<Product>()}, 1);
            var viewBag = ((PartialViewResult)result).ViewBag;
            var model = ((PartialViewResult)result).ViewData.Model as PayingItemModel;

            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
            Assert.AreEqual(model.Products.Count, 0);
            Assert.IsNotNull(model.PayingItem);
            Assert.AreEqual(0, viewBag.Categories.Count);
            Assert.AreEqual(0, viewBag.Accounts.Count);
        }
        
        [TestMethod]
        [TestCategory("PayingItemControllerTests")]
        public async Task Add_ValidModel_DateGreaterThanNow_ProductsNull_ReturnsRedirect()
        {
            //Arrange
            var month = DateTime.Today.Month + 1;
            var year = DateTime.Today.Year;            
            PayingItemModel pItemModel = new PayingItemModel()
            {
                PayingItem = new PayingItem() { AccountID = 1, CategoryID = 1, Date = new DateTime(year, month, 1), UserId = "1", ItemID = 1 },
                Products = null,
            };           
            var target = new PayingItemController(null, null, _payingItemService.Object, null, null);

            //Act
            var tmpResult = await target.Add(new WebUser() { Id = "1" }, pItemModel, 1);            

            //Assert
            _payingItemService.Verify(m => m.CreateAsync(It.IsAny<PayingItem>()), Times.Exactly(1));
            Assert.IsInstanceOfType(tmpResult, typeof(RedirectToRouteResult));
        }

        [TestMethod]
        [TestCategory("PayingItemControllerTests")]
        public async Task Add_ValidModel_ProductsNotNull_ReturnsRedirect()
        {
            PayingItemModel pItemModel = new PayingItemModel()
            {
                PayingItem = new PayingItem() { AccountID = 1, CategoryID = 1, Date = DateTime.Today, UserId = "1", ItemID = 1 },
                Products = new List<Product>()
            };            
            var target = new PayingItemController(_pItemProductHelper.Object, _payingItemHelper.Object, _payingItemService.Object, null, null);

            var result = await target.Add(new WebUser() { Id = "1" }, pItemModel, 2);

            _payingItemHelper.Verify(m => m.CreateCommentWhileAdd(pItemModel),Times.Exactly(1));
            _payingItemService.Verify(m => m.CreateAsync(It.IsAny<PayingItem>()), Times.Exactly(1));
            _pItemProductHelper.Verify(m => m.CreatePayingItemProduct(pItemModel), Times.Exactly(1));
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }

        [TestMethod]
        [TestCategory("PayingItemControllerTests")]
        public async Task Add_ValidModel_ProductsHaveSumm_ReturnsRedirect()
        {
            PayingItemModel pItemModel = new PayingItemModel()
            {
                PayingItem = new PayingItem() { AccountID = 1, CategoryID = 1, Date = DateTime.Today, UserId = "1", ItemID = 1 },
                Products = new List<Product>()
                {
                    new Product(){Price = 100},
                    new Product(){ Price = 200}
                }
            };
            var target = new PayingItemController(_pItemProductHelper.Object, _payingItemHelper.Object, _payingItemService.Object, null, null);

            var result = await target.Add(new WebUser() { Id = "1" }, pItemModel, 2);

            _payingItemHelper.Verify(m => m.CreateCommentWhileAdd(pItemModel), Times.Exactly(1));
            _payingItemService.Verify(m => m.CreateAsync(It.IsAny<PayingItem>()), Times.Exactly(1));
            _pItemProductHelper.Verify(m => m.CreatePayingItemProduct(pItemModel), Times.Exactly(1));
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }

        [TestMethod]
        [TestCategory("PayingItemControllerTests")]
        public async Task Edit_Cannot_Get_PayingItem_Returns_RedirectToRouteResult()
        {
            PayingItem pItem = null;
            _payingItemService.Setup(m => m.GetItemAsync(It.IsAny<int>())).ReturnsAsync(pItem);
            var target = new PayingItemController(null, null, _payingItemService.Object, _categoryService.Object, _accountService.Object);

            var result = await target.Edit(new WebUser() { Id = "1" }, 1, 5);
            var routes = (result as RedirectToRouteResult).RouteValues;

            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            Assert.AreEqual(routes["action"], "ListAjax");
        }

        [TestMethod]
        [TestCategory("PayingItemControllerTests")]
        public async Task Delete_RedirectsToActionList()
        {
            PayingItemController target = new PayingItemController(null, null, _payingItemService.Object, null, null);

            var result = await target.Delete(new WebUser(), It.IsAny<int>());
            var redirect = (RedirectToRouteResult)result;

            _payingItemService.Verify(m => m.DeleteAsync(It.IsAny<int>()), Times.Exactly(1));
            Assert.AreEqual(redirect.RouteValues["action"], "List");
        }

        [TestMethod]
        [TestCategory("PayingItemControllerTests")]
        public void List_ReturnsPartialView_With_PayingItemsByDate()
        {
            DateTime date = DateTime.Now - TimeSpan.FromDays(2);
            _payingItemService.Setup(m => m.GetList()).Returns(new List<PayingItem>()
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
                });
            PayingItemController target = new PayingItemController(null, null, _payingItemService.Object, null, null);

            var result = ((PartialViewResult)target.List(new WebUser() { Id = "1" })).Model as PayingItemToView;

            Assert.AreEqual(result.PayingItems.Count() == 3, true);
        }

        [TestMethod]
        [TestCategory("PayingItemControllerTests")]
        public void Can_Paginate()
        {
            DateTime date = DateTime.Now - TimeSpan.FromDays(2);
            _payingItemService.Setup(m => m.GetList()).Returns(new PayingItem[]
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
            });
            var target =
                new PayingItemController(null, null, _payingItemService.Object, null, null) {ItemsPerPage = 2};

            var pItemToView = ((PartialViewResult)target.List(new WebUser() { Id = "1" }, 2)).Model as PayingItemToView;
            var result = pItemToView?.PayingItems.ToArray();

            Assert.AreEqual(result.Count(), 2);
            Assert.AreEqual(result[0].AccountID, 1);
            Assert.AreEqual(result[1].AccountID, 2);
        }

        [TestMethod]
        [TestCategory("PayingItemControllerTests")]
        public async Task Edit_Get_Can_Get_PayingItem_For_Edit_With_SubCategories()
        {
            _payingItemService.Setup(x => x.GetItemAsync(It.IsAny<int>())).ReturnsAsync(new PayingItem() { CategoryID = 1});
            _payingItemService.Setup(x => x.GetList()).Returns(new List<PayingItem>(){new PayingItem(){CategoryID = 1}});
            var target = new PayingItemController(_pItemProductHelper.Object, _payingItemHelper.Object,
                _payingItemService.Object, _categoryService.Object, _accountService.Object);

            var result = await target.Edit(new WebUser() { Id = "1" }, 1, 1);
            var model = ((PartialViewResult)result).ViewData.Model as PayingItemEditModel;

            _pItemProductHelper.Verify(m => m.FillPayingItemEditModel(model, It.IsAny<int>()));
            Assert.AreEqual(PayingItemEditModel.OldCategoryId, 1);
            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
        }

        [TestMethod]
        [TestCategory("PayingItemControllerTests")]
        public async Task Edit_Get_ReturnsRedirectToListAjax()
        {
            var target = new PayingItemController(null, null, _payingItemService.Object, _categoryService.Object, _accountService.Object);

            var result = await target.Edit(new WebUser() { Id = "1" }, 1, 5);
            var redirectResult = (RedirectToRouteResult) result;

            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            Assert.AreEqual(redirectResult.RouteValues["action"], "ListAjax");
        }

        [TestMethod]
        [TestCategory("PayingItemControllerTests")]
        public async Task Edit_Get_WithOutSubCategories_ReturnsPartialView()
        {
            _payingItemService.Setup(x => x.GetList()).Returns(new List<PayingItem>());
            _payingItemService.Setup(x => x.GetItemAsync(It.IsAny<int>())).ReturnsAsync(new PayingItem() {ItemID = 6});
            var target = new PayingItemController(null, null, _payingItemService.Object, _categoryService.Object, _accountService.Object);

            var result = await target.Edit(new WebUser() { Id = "1" }, 1, 6);
            var model = ((PartialViewResult)result).ViewData.Model as PayingItemEditModel;

            Assert.AreEqual(model.PayingItem.ItemID, 6);
            Assert.AreEqual(model.PayingItemProducts.Count, 0);
            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
        }

        [TestMethod]
        [TestCategory("PayingItemControllerTests")]
        public async Task Edit_Post_InvalidModel_ReturnsPartialView()
        {
            _accountService.Setup(x => x.GetList()).Returns(new List<Account>());
            _categoryService.Setup(x => x.GetList()).Returns(new List<Category>());
            _categoryService.Setup(x => x.GetItemAsync(It.IsAny<int>())).ReturnsAsync(new Category(){TypeOfFlowID = 1});
            var target = new PayingItemController(null, null, null, _categoryService.Object, _accountService.Object);
            var pItemEditModel = new PayingItemEditModel() { PayingItem = new PayingItem() {}};
            target.ModelState.AddModelError("error", "error");

            //Action
            var result = await target.Edit(new WebUser() { Id = "1" }, pItemEditModel);

            //Assert
            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
        }

        [TestMethod]
        [TestCategory("PayingItemControllerTests")]
        public async Task Edit_Post_PricesAndIdsInItemAreNull_ReturnsRedirectToList()
        {
            var pItemEditModel = new PayingItemEditModel()
            {
                PayingItem = new PayingItem()
                {
                    CategoryID = 1,
                    AccountID = 1,
                    ItemID = 1
                },
                PricesAndIdsInItem = null
            };
            var target = new PayingItemController(null, null, _payingItemService.Object, null, null);

            var result = await target.Edit(new WebUser() { Id = "1" }, pItemEditModel);
            var redirectResult = (RedirectToRouteResult) result;

            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            Assert.AreEqual(redirectResult.RouteValues["action"], "List");
        }

        [TestMethod]
        [TestCategory("PayingItemControllerTests")]
        public async Task Edit_Post_SavePayingItemWithChangedCategory_RedirectsToList()
        {
            PayingItemEditModel.OldCategoryId = 1;
            var pItemEditModel = new PayingItemEditModel()
            {
                PricesAndIdsInItem = new List<PriceAndIdForEdit>(),
                PayingItem = new PayingItem() { CategoryID = 2}
            };
            var target = new PayingItemController(_pItemProductHelper.Object, _payingItemHelper.Object,
                _payingItemService.Object, _categoryService.Object, _accountService.Object);

            var result = await target.Edit(new WebUser() {Id = "1"}, pItemEditModel);
            var routeResult = (RedirectToRouteResult) result;

            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            Assert.AreEqual(routeResult.RouteValues["action"], "List");
            _payingItemService.Verify(m => m.UpdateAsync(pItemEditModel.PayingItem));
            _pItemProductHelper.Verify(m => m.CreatePayingItemProduct(pItemEditModel));
        }

        [TestMethod]
        [TestCategory("PayingItemControllerTests")]
        [ExpectedException(typeof(WebUiException))]
        public async Task Edit_RaisesServiceException()
        {
            _payingItemService.Setup(m => m.GetItemAsync(It.IsAny<int>())).Throws<ServiceException>();
            _categoryService.Setup(m => m.GetActiveGategoriesByUser(It.IsAny<string>()))
                .ReturnsAsync(new List<Category>());
            _accountService.Setup(m => m.GetListAsync()).ReturnsAsync(new List<Account>());
            var target = new PayingItemController(null, null, _payingItemService.Object, _categoryService.Object, _accountService.Object);

            await target.Edit(new WebUser(), 1, It.IsAny<int>());
        }

        [TestMethod]
        [TestCategory("PayingItemControllerTests")]
        public async Task Edit_RaisesWebUiExceptionWithInnerServiceException()
        {
            _payingItemService.Setup(m => m.GetItemAsync(It.IsAny<int>())).Throws<ServiceException>();
            _categoryService.Setup(m => m.GetActiveGategoriesByUser(It.IsAny<string>()))
                .ReturnsAsync(new List<Category>());
            _accountService.Setup(m => m.GetListAsync()).ReturnsAsync(new List<Account>());
            var target = new PayingItemController(null, null, _payingItemService.Object, _categoryService.Object, _accountService.Object);

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
        public async Task Edit_RaisesWebUiExceptionWithInnerWebUiException()
        {
            _payingItemService.Setup(m => m.GetList()).Throws<ServiceException>();
            _categoryService.Setup(m => m.GetActiveGategoriesByUser(It.IsAny<string>()))
                .ReturnsAsync(new List<Category>());
            _accountService.Setup(m => m.GetListAsync()).ReturnsAsync(new List<Account>());
            var target = new PayingItemController(null, null, _payingItemService.Object, _categoryService.Object, _accountService.Object);

            try
            {
                await target.Edit(new WebUser(), It.IsAny<int>(), It.IsAny<int>());
            }
            catch (Exception e)
            {
                Assert.IsInstanceOfType(e.InnerException, typeof(WebUiException));
            }
        }

        [TestMethod]
        [TestCategory("PayingItemControllerTests")]
        public async Task Can_Save_PayingItem_With_Same_Category()
        {
            PayingItemEditModel.OldCategoryId = 2;
            var pItemEditModel = new PayingItemEditModel()
            {
                PricesAndIdsInItem = new List<PriceAndIdForEdit>(),
                PayingItem = new PayingItem() { CategoryID = 2 }
            };
            var target = new PayingItemController(_pItemProductHelper.Object, _payingItemHelper.Object,
                _payingItemService.Object, _categoryService.Object, _accountService.Object);

            var result = await target.Edit(new WebUser() { Id = "1" }, pItemEditModel);
            var routeResult = (RedirectToRouteResult)result;

            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            Assert.AreEqual(routeResult.RouteValues["action"], "List");
            _payingItemService.Verify(m => m.UpdateAsync(pItemEditModel.PayingItem));
            _pItemProductHelper.Verify(m => m.UpdatePayingItemProduct(pItemEditModel));
        }

        [TestMethod]
        [TestCategory("PayingItemControllerTests")]
        public void ExpensiveCategories()
        {
            //Arrange
            _payingItemService.Setup(m => m.GetList()).Returns(new List<PayingItem>()
                {
                    new PayingItem() {UserId = "1",Category = new Category() {TypeOfFlowID = 2,Name = "Cat1"},Date = DateTime.Now, Summ = 100},
                    new PayingItem() {UserId = "1",Category = new Category() {TypeOfFlowID = 2,Name = "Cat2"},Date = DateTime.Now, Summ = 200},
                    new PayingItem() {UserId = "2",Category = new Category() {TypeOfFlowID = 2},Date = DateTime.Now},
                    new PayingItem() {UserId = "2",Category = new Category() {TypeOfFlowID = 12},Date = DateTime.Now},
                    new PayingItem() {UserId = "1",Category = new Category() {TypeOfFlowID = 12},Date = DateTime.Now},
                });
            WebUser user = new WebUser() { Id = "1" };
            PayingItemController target = new PayingItemController(null, null, _payingItemService.Object, null, null);

            //Act
            var result = ((PartialViewResult)target.ExpensiveCategories(user)).ViewData.Model as List<OverAllItem>;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result[0].Category, "Cat2");
            Assert.AreEqual(result[1].Category, "Cat1");
            Assert.AreEqual(result.Count, 2);
        }
    }
}