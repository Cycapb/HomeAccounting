using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.WebPages;
using DomainModels.Abstract;
using DomainModels.Model;
using DomainModels.Repositories;
using HomeAccountingSystem_WebUI.Abstract;
using HomeAccountingSystem_WebUI.Controllers;
using HomeAccountingSystem_WebUI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace WebUI.Tests
{
    //public class TestMockObject
    //{
    //    private readonly int _payingItemId;

    //    public IRepository<Product> MockProductObject { get; private set; }
    //    public IRepository<PayingItem> MockPayingItemObject { get; private set; }
    //    public IRepository<Account> MockAccountObject { get; private set; }
    //    public IRepository<Category> MockCategoryObject { get; private set; }
    //    public IRepository<PaiyngItemProduct> MockPayingItemProductObject { get; set; }
    //    public IRepository<PlanItem> MockPlanItemObject { get; private set; } 

    //    public Mock<IRepository<PayingItem>> MockPayingItem { get; private set; } 
    //    public Mock<IRepository<PaiyngItemProduct>> MockPayingItemProduct { get; set; }
    //    public Mock<IRepository<Product>> MockProduct { get; set; }

    //    public TestMockObject (int payingItemId = 1)
    //    {
    //        _payingItemId = payingItemId;
    //        FillMockAccount();
    //        FillMockCategory();
    //        FillMockPayingItem();
    //        FillMockProduct();
    //        FillMockPayingItemProduct();
    //        FillMockPlanItem();
    //    }

    //    private void FillMockProduct()
    //    {
    //        var mockProduct = new Mock<IRepository<Product>>();
    //        mockProduct.Setup(m => m.GetList()).Returns(new List<Product>()
    //        {
    //            new Product() {CategoryID = 1, Description = "Prod1",ProductID = 1,UserID = "1"},
    //            new Product() {CategoryID = 1, Description = "Prod2",ProductID = 2,UserID = "1"},
    //            new Product() {CategoryID = 2, Description = "Prod3",ProductID = 3,UserID = "2"},
    //            new Product() {CategoryID = 2, Description = "Prod5",ProductID = 5,UserID = "2"},
    //            new Product() {CategoryID = 2, Description = "Prod6",ProductID = 6,UserID = "2"},
    //            new Product() {CategoryID = 2, Description = "Prod7",ProductID = 7,UserID = "2"}
    //        });
    //        MockProduct = mockProduct;
    //        MockProductObject = mockProduct.Object;
    //    }

    //    private void FillMockPayingItem()
    //    {
    //        var mockPayingItem = new Mock<IRepository<PayingItem>>();
    //        mockPayingItem.Setup(m => m.GetList()).Returns(new List<PayingItem>()
    //        {
    //            new PayingItem() {ItemID = 1, AccountID = 1, CategoryID = 1, UserId = "1"},
    //            new PayingItem() {ItemID = 2, AccountID = 1, CategoryID = 1, UserId = "1"},
    //            new PayingItem() {ItemID = 3, AccountID = 1, CategoryID = 1, UserId = "1"},
    //            new PayingItem() {ItemID = 4, AccountID = 1, CategoryID = 1, UserId = "2"},
    //            new PayingItem() {ItemID = 6, AccountID = 1, CategoryID = 3, UserId = "2"}
    //        });
    //        MockPayingItem = mockPayingItem;
    //        MockPayingItemObject = mockPayingItem.Object;
    //            mockPayingItem.Setup(m => m.GetItemAsync(_payingItemId))
    //                .ReturnsAsync(MockPayingItemObject.GetList().Single(x => x.ItemID == _payingItemId) as PayingItem);

    //    }

    //    private void FillMockAccount()
    //    {
    //        var mockAccount = new Mock<IRepository<Account>>();
    //        mockAccount.Setup(m => m.GetList()).Returns(new List<Account>
    //        {
    //            new Account() {AccountID = 1, UserId = "1"},
    //            new Account() {AccountID = 2, UserId = "1"},
    //        });
    //        MockAccountObject = mockAccount.Object;

    //    }

    //    private void FillMockCategory()
    //    {
    //        var mockCategory = new Mock<IRepository<Category>>();
    //        mockCategory.Setup(m => m.GetList()).Returns(new List<Category>()
    //        {
    //            new Category() {CategoryID = 1,Name = "Cat1",TypeOfFlowID = 1, UserId = "1"},
    //            new Category() {CategoryID = 2,Name = "Cat2",TypeOfFlowID = 1, UserId = "1"},
    //        });

    //        MockCategoryObject = mockCategory.Object;
    //        mockCategory.Setup(m => m.GetItem(1)).Returns(new Category() {CategoryID = 1, TypeOfFlowID = 1});
    //    }

    //    private void FillMockPlanItem()
    //    {
    //        var mock = new Mock<IRepository<PlanItem>>();
    //        mock.Setup(m => m.GetList()).Returns(new List<PlanItem>
    //        {
    //            new PlanItem() { CategoryId = 1,Closed = false,Month = DateTime.Today},
    //            new PlanItem() { CategoryId = 2,Closed = false,Month = DateTime.Today}
    //        });
    //        MockPlanItemObject = mock.Object;
    //    }

    //    private void FillMockPayingItemProduct()
    //    {
    //        var list = new List<PaiyngItemProduct>()
    //        {
    //            new PaiyngItemProduct() {ItemID = 1, PayingItemID = 1, ProductID = 1},
    //            new PaiyngItemProduct() {ItemID = 2, PayingItemID = 1, ProductID = 3},
    //            new PaiyngItemProduct() {ItemID = 3, PayingItemID = 2, ProductID = 1},
    //            new PaiyngItemProduct() {ItemID = 4, PayingItemID = 3, ProductID = 1},
    //        };
    //        var mockPayingItemProduct = new Mock<IRepository<PaiyngItemProduct>>();
    //        mockPayingItemProduct.Setup(m => m.GetList()).Returns(list);
    //        MockPayingItemProductObject = mockPayingItemProduct.Object;
    //        MockPayingItemProduct = mockPayingItemProduct;
    //        mockPayingItemProduct.Setup(m => m.GetItemAsync(It.IsAny<int>()))
    //.ReturnsAsync(this.MockPayingItemProductObject.GetList().Single(x => x.ItemID == 1));
    //    }
    //}
    
    //[TestClass]
    //public class PayingItemTest
    //{
    //    private PayingItemEditModel CreatePayingItemEditModel(bool pricesAndIdsNotInItem)
    //    {
    //        var pItemEditModel = new PayingItemEditModel()
    //        {
    //         PayingItem   = new PayingItem()
    //         {
    //             AccountID = 1,
    //             CategoryID = 1,
    //             ItemID = 1,
    //             UserId = "1"
    //         },
    //         PricesAndIdsInItem = new List<PriceAndIdForEdit>()
    //         {
    //             new PriceAndIdForEdit() {Id = 5,PayingItemProductId = 1,Price = 100M},
    //             new PriceAndIdForEdit() {Id = 6,PayingItemProductId = 2,Price = 200M},
    //             new PriceAndIdForEdit() {Id = 0,PayingItemProductId = 0,Price = 100M},
    //             new PriceAndIdForEdit() {Id = 7,PayingItemProductId = 1,Price = 200M},
    //             new PriceAndIdForEdit() {Id = 0,PayingItemProductId = 0,Price = 200M}
    //         }
    //        };
    //        if (pricesAndIdsNotInItem)
    //        {
    //            pItemEditModel.PricesAndIdsNotInItem = new List<PriceAndIdForEdit>()
    //            {
    //                new PriceAndIdForEdit() {Id = 2, PayingItemProductId = 0, Price = 200M},
    //                new PriceAndIdForEdit() {Id = 0, PayingItemProductId = 0, Price = 200M}
    //            };
    //        }
    //        else
    //        {
    //            pItemEditModel.PricesAndIdsNotInItem = null;
    //        }
    //        return pItemEditModel;
    //    }

    //    [TestMethod]
    //    public void Can_Create_PayingItem_For_Adding()
    //    {
    //        var mock = new TestMockObject();
    //        var target = new PayingItemController(null,mock.MockCategoryObject,mock.MockAccountObject,null,null,null);

    //        var result = target.Add(new WebUser() {Id = "1"}, 1);
    //        var model = ((PartialViewResult) result).ViewData.Model as PayingItemModel;
    //        var viewBag = ((PartialViewResult) result).ViewBag;

    //        Assert.IsInstanceOfType(result,typeof(PartialViewResult));
    //        Assert.AreEqual(model.Products.Count,0);
    //        Assert.AreEqual(model.Products.Count,0);
    //        Assert.IsNotNull(model.PayingItem);
    //        Assert.AreEqual(2,viewBag.Categories.Count);
    //    }

    //    [TestMethod]
    //    public async Task Can_Add_Valid_PayingItem_()
    //    {
    //        DateTime date = DateTime.Now - TimeSpan.FromDays(2);
            
    //        //Arrange
    //        Mock<IRepository<PayingItem>> mock = new Mock<IRepository<PayingItem>>();
    //        var testmock = new TestMockObject();
    //        PayingItemModel pItemModel = new PayingItemModel()
    //        {
    //            PayingItem = new PayingItem() { AccountID = 1, CategoryID = 1,Date = date,UserId = "1", ItemID = 1},
    //            Products = null,
    //        };
    //        int typeOfFlow = 1;
    //        var user = new WebUser() {Id = "1"};
    //        var target = new PayingItemController(mock.Object,testmock.MockCategoryObject,testmock.MockAccountObject,null,null,null);

    //        //Action
    //        var tmpResult = await target.Add(user, pItemModel, typeOfFlow);
            
    //        //Assert
    //        Assert.IsNotInstanceOfType(tmpResult,typeof(ViewResult));
    //    }

    //    [TestMethod]
    //    public async Task Can_Add_Valid_PayingItem_With_PayingItemProduct()
    //    {
    //        var mock = new TestMockObject();
    //        Mock<IRepository<PayingItem>> mockPayingItem = new Mock<IRepository<PayingItem>>();
    //        Mock<IPayingItemHelper> mockPayingItemHelper = new Mock<IPayingItemHelper>();
    //        Mock<IPayingItemProductHelper> mockPAyingItemProductHelper = new Mock<IPayingItemProductHelper>();
    //        PayingItemModel pItemModel = new PayingItemModel()
    //        {
    //            PayingItem = new PayingItem() { AccountID = 1, CategoryID = 1, Date = DateTime.Today, UserId = "1", ItemID = 1 },
    //            Products = new List<Product>()
    //        };
    //        WebUser user = new WebUser() {Id = "1"};
    //        var target = new PayingItemController(mockPayingItem.Object,null,mock.MockAccountObject,mock.MockProductObject,mockPAyingItemProductHelper.Object, mockPayingItemHelper.Object);

    //        var result = await target.Add(user, pItemModel, 2);
            
    //        mockPayingItem.Verify(m=>m.SaveAsync());
    //        mockPAyingItemProductHelper.Verify(m=>m.CreatePayingItemProduct(pItemModel));
    //        Assert.IsInstanceOfType(result,typeof(RedirectToRouteResult));
    //    }

    //    [TestMethod]
    //    public async Task Cannot_Add_Invalid_PayingItem()
    //    {
    //        var mockPlanItem = new TestMockObject();
    //        Mock<IRepository<PayingItem>> mock = new Mock<IRepository<PayingItem>>();
    //        Mock<IRepository<Category>> mockCategory = new Mock<IRepository<Category>>();
    //        Mock<IRepository<Account>> mockAccount = new Mock<IRepository<Account>>();
    //        mockAccount.Setup(m => m.GetList()).Returns(new List<Account>
    //        {
    //            new Account() {AccountID = 1, UserId = "1"},
    //            new Account() {AccountID = 2, UserId = "1"},
    //        });
    //        mockCategory.Setup(m => m.GetList()).Returns(new List<Category>()
    //        {
    //            new Category() {CategoryID = 1,Name = "Cat1",TypeOfFlowID = 1},
    //            new Category() {CategoryID = 2,Name = "Cat2",TypeOfFlowID = 1},
    //        });
    //        PayingItemModel pItemModel = new PayingItemModel()
    //        {
    //            PayingItem = new PayingItem() { AccountID = 1, CategoryID = 1, Date = DateTime.Today, UserId = "1", ItemID = 1 },
    //            Products = null,
    //        };
    //        int typeOfFlow = 1;
    //        var user = new WebUser() { Id = "1" };
    //        var target = new PayingItemController(mock.Object, mockCategory.Object, mockAccount.Object, null, null,null);

    //        target.ModelState.AddModelError("error","error");
    //        var result = await target.Add(user, pItemModel, typeOfFlow);

    //        mock.Verify(m=>m.SaveAsync(),Times.Never);
    //        Assert.IsInstanceOfType(result,typeof(PartialViewResult));
    //    }

    //    [TestMethod]
    //    public void Can_Get_PayingItems_By_Date()
    //    {
    //        DateTime date = DateTime.Now - TimeSpan.FromDays(2);
    //        var testMock = new TestMockObject();
    //        Mock<IRepository<PayingItem>> mock = new Mock<IRepository<PayingItem>>();
    //        mock.Setup(m => m.GetList()).Returns(new List<PayingItem>()
    //        {
    //            new PayingItem()
    //            {
    //                AccountID = 1,CategoryID = 1,Comment = "PayingItem 1",Date = "22.11.2015".AsDateTime(),
    //                Category = new Category() {Name = "Cat1"}
    //            },
    //            new PayingItem()
    //            {
    //                AccountID = 2,CategoryID = 1,Comment = "PayingItem 2",Date = date,UserId = "1",
    //                Category = new Category() {Name = "Cat2"}
    //            },
    //            new PayingItem()
    //            {
    //                AccountID = 3,CategoryID = 2,Comment = "PayingItem 3",Date = date,
    //                UserId = "1",Category = new Category() {Name = "Cat3"}
    //            },
    //            new PayingItem()
    //            {
    //                AccountID = 4,CategoryID = 2,Comment = "PayingItem 4",Date = date,UserId = "1",
    //                Category = new Category() {Name = "Cat4"}
    //            }
    //        });
    //        PayingItemController target = new PayingItemController(mock.Object, null, null,null,null,null);

    //        var result = ((PartialViewResult)target.List(new WebUser() {Id = "1"},1)).Model as PayingItemToView;

    //        Assert.AreEqual(result.PayingItems.Count() == 3,true);
    //    }

    //    [TestMethod]
    //    public void Can_Paginate()
    //    {
    //        DateTime date = DateTime.Now - TimeSpan.FromDays(2);
    //        var testMock = new TestMockObject();
    //        Mock<IRepository<PayingItem>> mock = new Mock<IRepository<PayingItem>>();
    //        mock.Setup(m => m.GetList()).Returns(new PayingItem[]
    //        {
    //            new PayingItem()
    //            {
    //                AccountID = 4,CategoryID = 4,Comment = "PayingItem 4",Date = date,UserId = "1",
    //                Category = new Category() {Name = "Cat1"}
    //            },
    //            new PayingItem()
    //            {
    //                AccountID = 1,CategoryID = 1,Comment = "PayingItem 1",Date = date,UserId = "1",
    //                Category = new Category() {Name = "Cat2"}
    //            },
    //            new PayingItem()
    //            {
    //                AccountID = 2,CategoryID = 2,Comment = "PayingItem 2",Date = date,UserId = "1",
    //                Category = new Category() {Name = "Cat3"}
    //            },
    //            new PayingItem()
    //            {
    //                AccountID = 3,CategoryID = 2,Comment = "PayingItem 3",Date = DateTime.Now - TimeSpan.FromDays(1),UserId = "1",
    //                Category = new Category() {Name = "Cat4"}
    //            }
    //        });
    //        PayingItemController target = new PayingItemController(mock.Object, null, null, null, null,null);
    //        target.ItemsPerPage = 2;

    //        PayingItemToView pItemToView = ((PartialViewResult)target.List(new WebUser() {Id = "1"},2)).Model as PayingItemToView;
    //        PayingItem[] result = pItemToView.PayingItems.ToArray(); 

    //        Assert.AreEqual(result.Count(),2);
    //        Assert.AreEqual(result[0].AccountID,1);
    //        Assert.AreEqual(result[1].AccountID,2);
    //    }

    //    [TestMethod]
    //    public async Task Can_Get_PayingItem_For_Edit_With_SubCategories()
    //    {
    //        var testMock = new TestMockObject();
    //        Mock<IPayingItemHelper> mockPayingItemHelper = new Mock<IPayingItemHelper>();
    //        Mock<IPayingItemProductHelper> mockPayingItemProductHelper = new Mock<IPayingItemProductHelper>();
    //        var target = new PayingItemController(testMock.MockPayingItemObject,testMock.MockCategoryObject,
    //            testMock.MockAccountObject,testMock.MockProductObject,mockPayingItemProductHelper.Object,mockPayingItemHelper.Object);

    //        var result = await target.Edit(new WebUser() {Id = "1"}, 1, 1);
    //        var model = ((PartialViewResult)result).ViewData.Model as PayingItemEditModel;

    //        mockPayingItemProductHelper.Verify(m=>m.FillPayingItemEditModel(model,It.IsAny<int>()));
    //        Assert.AreEqual(PayingItemEditModel.OldCategoryId,1);
    //        Assert.IsInstanceOfType(result,typeof(PartialViewResult));
    //    }

    //    [TestMethod]
    //    public async Task Cannot_Get_Paying_For_Edit()
    //    {
    //        var testMock = new TestMockObject();
    //        var target = new PayingItemController(testMock.MockPayingItemObject, testMock.MockCategoryObject,
    //            testMock.MockAccountObject, testMock.MockProductObject, null,null);

    //        var result = await target.Edit(new WebUser() {Id = "1"}, 1, 5);
            
    //        Assert.IsInstanceOfType(result,typeof(ActionResult));
    //    }

    //    [TestMethod]
    //    public async Task Can_Get_PayingItem_For_Edit_WithOut_SubCategories()
    //    {
    //        var testmock = new TestMockObject(6);
    //        var target = new PayingItemController(testmock.MockPayingItemObject, testmock.MockCategoryObject, testmock.MockAccountObject,
    //            testmock.MockProductObject,null,null);

    //        var result = await target.Edit(new WebUser() { Id = "1" }, 1, 6);
    //        var model = ((PartialViewResult)result).ViewData.Model as PayingItemEditModel;

    //        Assert.AreEqual(model.PayingItem.ItemID,6);
    //        Assert.AreEqual(model.PayingItemProducts.Count, 0);
    //        Assert.IsInstanceOfType(result,typeof(PartialViewResult));
    //    }

    //    [TestMethod]
    //    public async Task Cannot_Save_Invalid_PayingItem_Changes()
    //    {
    //        var testmock = new TestMockObject();
    //        PayingItemController target = new PayingItemController(testmock.MockPayingItemObject, testmock.MockCategoryObject, testmock.MockAccountObject, null, null,null);
    //        PayingItem pItem = new PayingItem() { ItemID = 1, UserId = "1", Comment = "Test", CategoryID = 1 };
    //        PayingItemEditModel pItemEditModel = new PayingItemEditModel() {PayingItem = pItem};
    //        target.ModelState.AddModelError("error", "error");

    //        //Action
    //        var result = await target.Edit(new WebUser() { Id = "1" }, pItemEditModel);

    //        //Assert
    //        testmock.MockPayingItem.Verify(m => m.SaveAsync(), Times.Never());
    //        Assert.IsInstanceOfType(result, typeof(PartialViewResult));
    //    }

    //    [TestMethod]
    //    public async Task Can_Save_PayingItem_Without_PricesAndIdsInItem()
    //    {
    //        var mock = new TestMockObject();
    //        var pItemEditModel = new PayingItemEditModel()
    //        {
    //            PayingItem = new PayingItem()
    //            {
    //                CategoryID = 1, AccountID = 1,ItemID = 1
    //            },
    //            PricesAndIdsInItem = null
    //        };
    //        var target = new PayingItemController(mock.MockPayingItemObject,mock.MockCategoryObject,mock.MockAccountObject,null,null,null);

    //        var result = await target.Edit(new WebUser() {Id = "1"}, pItemEditModel);
            
    //        mock.MockPayingItem.Verify(m=>m.UpdateAsync(pItemEditModel.PayingItem));
    //        mock.MockPayingItem.Verify(m=>m.SaveAsync());
    //        Assert.IsInstanceOfType(result,typeof(RedirectToRouteResult));
    //    }

    //    [TestMethod]
    //    public async Task Can_Save_PayingItem_With_ChangedCategory()
    //    {
    //        Mock<IPayingItemHelper> mockPayingItemHelper = new Mock<IPayingItemHelper>();
    //        Mock<IPayingItemProductHelper> mockPayingItemProductHelper = new Mock<IPayingItemProductHelper>();
    //        var mock = new TestMockObject();
    //        var pItemEditModel = CreatePayingItemEditModel(false);
    //        PayingItemEditModel.OldCategoryId = pItemEditModel.PayingItem.CategoryID;
    //        pItemEditModel.PayingItem.CategoryID = 2;
    //        var target = new PayingItemController(mock.MockPayingItemObject, mock.MockCategoryObject, mock.MockAccountObject, mock.MockProductObject, 
    //            mockPayingItemProductHelper.Object,mockPayingItemHelper.Object);

    //        var result = await target.Edit(new WebUser() { Id = "1" }, pItemEditModel);
    //        var routeResult = (RedirectToRouteResult) result;

    //        Assert.IsInstanceOfType(result, typeof(ActionResult));
    //        Assert.AreEqual(routeResult.RouteValues["action"],"List");
    //        mock.MockPayingItem.Verify(m => m.UpdateAsync(pItemEditModel.PayingItem));
    //        mock.MockPayingItem.Verify(m => m.SaveAsync());
    //        Assert.AreEqual(pItemEditModel.PayingItem.Summ, 500);
    //    }

    //    [TestMethod]
    //    public async Task Can_Save_PayingItem_With_Same_Category()
    //    {
    //        Mock<IPayingItemHelper> mockPayingItemHelper = new Mock<IPayingItemHelper>();
    //        Mock<IPayingItemProductHelper> mockPayingItemProductHelper = new Mock<IPayingItemProductHelper>();
    //        var mock = new TestMockObject();
    //        mock.MockProduct.Setup(m => m.GetList()).Returns(new List<Product>()
    //        {
    //            new Product() {CategoryID = 1, Description = "Prod1",ProductID = 1,UserID = "1"},
    //            new Product() {CategoryID = 1, Description = "Prod2",ProductID = 2,UserID = "1"},
    //            new Product() {CategoryID = 2, Description = "Prod3",ProductID = 3,UserID = "2"},
    //            new Product() {CategoryID = 1, Description = "Prod5",ProductID = 5,UserID = "2"},
    //            new Product() {CategoryID = 1, Description = "Prod6",ProductID = 6,UserID = "2"},
    //            new Product() {CategoryID = 1, Description = "Prod7",ProductID = 7,UserID = "2"}
    //        });
    //        var pItemEditModel = CreatePayingItemEditModel(true);
    //        PayingItemEditModel.OldCategoryId = pItemEditModel.PayingItem.CategoryID;
    //        var target = new PayingItemController(mock.MockPayingItemObject, mock.MockCategoryObject, mock.MockAccountObject, 
    //            mock.MockProductObject, mockPayingItemProductHelper.Object,mockPayingItemHelper.Object);

    //        var result = await target.Edit(new WebUser() {Id = "1"}, pItemEditModel);
    //        var routeResult = (RedirectToRouteResult) result;

    //        Assert.AreEqual(routeResult.RouteValues["action"],"List");
    //        Assert.IsInstanceOfType(result,typeof(ActionResult));
    //    }

    //    [TestMethod]
    //    public async Task Can_Delete_PayingItem()
    //    {
    //        var testMock = new TestMockObject();
    //        Mock<IRepository<PayingItem>> mock = new Mock<IRepository<PayingItem>>();
    //        mock.Setup(m => m.GetList()).Returns(new List<PayingItem>()
    //        {
    //            new PayingItem() {AccountID = 1, CategoryID = 1, ItemID = 1},
    //            new PayingItem() {AccountID = 1, CategoryID = 1, ItemID = 2},
    //            new PayingItem() {AccountID = 1, CategoryID = 1, ItemID = 3},
    //            new PayingItem() {AccountID = 1, CategoryID = 1, ItemID = 4},
    //        });
    //        var idToDelete = 3;

    //        PayingItemController target = new PayingItemController(mock.Object,testMock.MockCategoryObject,testMock.MockAccountObject, null, null,null);

    //        await target.Delete(new WebUser(), idToDelete);
    //        var result = mock.Object.GetList().ToList();

    //        mock.Verify(m=>m.DeleteAsync(idToDelete));
    //    }

    //    [TestMethod]
    //    public void CanGetExpensiveCategories()
    //    {
    //        //Arrange
    //        Mock<IRepository<PayingItem>> mock = new Mock<IRepository<PayingItem>>();
    //        mock.Setup(m => m.GetList()).Returns(new List<PayingItem>()
    //        {
    //            new PayingItem() {UserId = "1",Category = new Category() {TypeOfFlowID = 2,Name = "Cat1"},Date = DateTime.Now},
    //            new PayingItem() {UserId = "1",Category = new Category() {TypeOfFlowID = 2,Name = "Cat2"},Date = DateTime.Now},
    //            new PayingItem() {UserId = "2",Category = new Category() {TypeOfFlowID = 2},Date = DateTime.Now},
    //            new PayingItem() {UserId = "2",Category = new Category() {TypeOfFlowID = 12},Date = DateTime.Now},
    //            new PayingItem() {UserId = "1",Category = new Category() {TypeOfFlowID = 12},Date = DateTime.Now},
    //        });
    //        WebUser user = new WebUser() {Id = "1"};
    //        PayingItemController target = new PayingItemController(mock.Object,null,null, null, null,null);
            
    //        //Act
    //        var result = ((PartialViewResult)target.ExpensiveCategories(user)).ViewData.Model as List<OverAllItem>;

    //        //Assert
    //        Assert.IsNotNull(result);
    //        Assert.AreEqual(result[0].Category,"Cat1");
    //        Assert.AreEqual(result[1].Category,"Cat2");
    //        Assert.AreEqual(result.Count,2);
    //    }

    //    [TestMethod]
    //    public void Can_Get_SubCategories()
    //    {
    //        var mock = new TestMockObject();
    //        var categoryId = 1;
    //        var target = new PayingItemController(null,null,null,mock.MockProductObject,null,null);

    //        var result = target.GetSubCategories(categoryId);
    //        var model = ((PartialViewResult) result).ViewData.Model as List<Product>;

    //        Assert.AreEqual(model?.Count,2);
    //        Assert.AreEqual(model?[0].ProductID,1);
    //        Assert.AreEqual(model?[1].ProductID,2);
    //    }

    //    [TestMethod]
    //    public void Can_Get_SubCategoriesForEdit()
    //    {
    //        var mock = new TestMockObject();
    //        var target = new PayingItemController(null,null,null,mock.MockProductObject,null,null);

    //        var result = target.GetSubCategoriesForEdit(1);
    //        var model = ((PartialViewResult) result).ViewData.Model as List<Product>;

    //        Assert.AreEqual(model?.Count,2);
    //        Assert.IsInstanceOfType(result,typeof(PartialViewResult));
    //    }
    //}
}
