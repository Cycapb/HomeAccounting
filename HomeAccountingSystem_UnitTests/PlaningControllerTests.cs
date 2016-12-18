using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using HomeAccountingSystem_DAL.Model;
using HomeAccountingSystem_WebUI.Abstract;
using HomeAccountingSystem_WebUI.Controllers;
using HomeAccountingSystem_WebUI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Services;

namespace HomeAccountingSystem_UnitTests
{
    [TestClass]
    public class PlaningControllerTests
    {
        private ControllerContext GetControllerContext(Controller target)
        {
            Mock<HttpRequestBase> mockHttpRequest = new Mock<HttpRequestBase>();
            mockHttpRequest.SetupGet(x => x.Headers).Returns(new WebHeaderCollection() { "X-Requested-With:", "XMLHttpRequest:" });
            Mock<HttpContextBase> mockHttpContext = new Mock<HttpContextBase>();
            mockHttpContext.SetupGet(x => x.Request).Returns(mockHttpRequest.Object);
            return new ControllerContext(mockHttpContext.Object, new RouteData(), target);
        }

        private List<Category> _categories { get; } = new List<Category>()
        {
            new Category() {CategoryID = 1, Name = "Cat1", TypeOfFlowID = 1,UserId = "1"},
            new Category() {CategoryID = 2, Name = "Cat1", TypeOfFlowID = 1,UserId = "1"},
            new Category() {CategoryID = 3, Name = "Cat1", TypeOfFlowID = 1,UserId = "1"},
            new Category() {CategoryID = 4, Name = "Cat1", TypeOfFlowID = 1,UserId = "1"},
            new Category() {CategoryID = 5, Name = "Cat1", TypeOfFlowID = 1,UserId = "1"},
            new Category() {CategoryID = 6, Name = "Cat1", TypeOfFlowID = 2,UserId = "1"},
            new Category() {CategoryID = 7, Name = "Cat1", TypeOfFlowID = 2,UserId = "1"}
        };

        private List<PlanItem> _planItems { get; } = new List<PlanItem>()
        {
            new PlanItem() {CategoryId = 1,PlanItemID = 1,UserId = "1", Category = new Category() {CategoryID = 1,TypeOfFlowID = 1} },
            new PlanItem() {CategoryId = 1,PlanItemID = 3,UserId = "1",Category = new Category() {CategoryID = 2,TypeOfFlowID = 1} },
            new PlanItem() {CategoryId = 1,PlanItemID = 2,UserId = "1",Category = new Category() {CategoryID = 3,TypeOfFlowID = 1}},
            new PlanItem() {CategoryId = 2,PlanItemID = 4,UserId = "1",Category = new Category() {CategoryID = 4,TypeOfFlowID = 1}},
            new PlanItem() {CategoryId = 3,PlanItemID = 5,UserId = "1",Category = new Category() {CategoryID = 5,TypeOfFlowID = 1}},
            new PlanItem() {CategoryId = 6,PlanItemID = 4,UserId = "1",Category = new Category() {CategoryID = 6,TypeOfFlowID = 2}},
            new PlanItem() {CategoryId = 7,PlanItemID = 5,UserId = "1",Category = new Category() {CategoryID = 7,TypeOfFlowID = 2}},
        };

        private List<PayingItem> _payingItems { get; }  = new List<PayingItem>()
        {
            new PayingItem() {ItemID = 1,CategoryID = 1,UserId = "1"},
            new PayingItem() {ItemID = 2,CategoryID = 1,UserId = "1"},
            new PayingItem() {ItemID = 3,CategoryID = 1,UserId = "1"},
            new PayingItem() {ItemID = 4,CategoryID = 2,UserId = "2"},
            new PayingItem() {ItemID = 5,CategoryID = 3,UserId = "3"},
        };

        private async Task Prepare_UserWithNoCategories_PayingItemListReturned(WebUser user)
        {
            Mock<ICategoryService> mockCategory = new Mock<ICategoryService>();
            Mock<IPlanItemService> mockPlanItem = new Mock<IPlanItemService>();
            mockPlanItem.Setup(m => m.GetListAsync(It.IsAny<string>())).ReturnsAsync(new List<PlanItem>());
            mockCategory.Setup(m => m.GetListAsync()).ReturnsAsync(new List<Category>());
            var target = new PlaningController(mockCategory.Object,mockPlanItem.Object, null);
            
            var result = await target.Prepare(user);

            var controller = ((RedirectToRouteResult)result).RouteValues["controller"];
            var action = ((RedirectToRouteResult)result).RouteValues["action"];

            Assert.AreEqual("Index",action);
            Assert.AreEqual("PayingItem",controller);
        }

        private async Task MethodPrepare_UserWithCategoriesWithNoPlanItems_CreatePlanReturned(WebUser user)
        {
            Mock<IPlanItemService> mockPlanItem = new Mock<IPlanItemService>();
            Mock<ICategoryService> mockCategory = new Mock<ICategoryService>();
            mockCategory.Setup(m => m.GetListAsync()).ReturnsAsync(_categories);
            var target = new PlaningController(mockCategory.Object, mockPlanItem.Object, null);

            var result = await target.Prepare(user);
            var controller = ((RedirectToRouteResult)result).RouteValues["controller"];
            var action = ((RedirectToRouteResult)result).RouteValues["action"];

            Assert.IsNull(controller);
            Assert.AreEqual("CreatePlan",action);
        }

        private async Task MethodPrepare_WithCategories_WithPlanItems(WebUser user)
        {
            Mock<IPlanItemService> mockPlanItem = new Mock<IPlanItemService>();
            Mock<ICategoryService> mockCategory = new Mock<ICategoryService>();
            mockCategory.Setup(m => m.GetListAsync()).ReturnsAsync(_categories);
            mockPlanItem.Setup(m => m.GetListAsync(It.IsAny<string>())).ReturnsAsync(_planItems);
            var target = new PlaningController(mockCategory.Object, mockPlanItem.Object, null);

            var result = await target.Prepare(user);
            var controller = ((RedirectToRouteResult)result).RouteValues["controller"];
            var action = ((RedirectToRouteResult)result).RouteValues["action"];

            Assert.IsNull(controller);
            Assert.AreEqual("ViewPlan", action);
        }

        private async Task CreatePlan_RedirectToViewPlanReturned(WebUser user)
        {
            Mock<IPlanningHelper> helperMock = new Mock<IPlanningHelper>();
            Mock<IPlanItemService> mockPlanItem = new Mock<IPlanItemService>();
            Mock<ICategoryService> mockCategory = new Mock<ICategoryService>();
            var target = new PlaningController(mockCategory.Object,mockPlanItem.Object,helperMock.Object);

            var result = await target.CreatePlan(user);

            helperMock.Verify(m=>m.CreatePlanItems(user));
            Assert.AreEqual(result.RouteValues["action"],"ViewPlan");
        }

        private async Task ViewPlan_ViewResultReturned(WebUser user)
        {
            Mock<IPlanningHelper> helperMock = new Mock<IPlanningHelper>();
            Mock<IPlanItemService> mockPlanItem = new Mock<IPlanItemService>();
            Mock<ICategoryService> mockCategory = new Mock<ICategoryService>();
            helperMock.Setup(m => m.GetUserBalance(It.IsAny<WebUser>(),It.IsAny<bool>())).ReturnsAsync(new ViewPlaningModel());
            var target = new PlaningController(mockCategory.Object,mockPlanItem.Object,helperMock.Object);

            var result = await target.ViewPlan(user);
            
            Assert.IsNotNull(target.ViewData.Model);
            Assert.IsInstanceOfType(result,typeof(ViewResult));
        }

        private async void Edit_IdInput_PartialViewResulEditPlaningModeltReturned()
        {
            Mock<IPlanItemService> mockPlanItem = new Mock<IPlanItemService>();
            mockPlanItem.Setup(m => m.GetItemAsync(It.IsAny<int>())).ReturnsAsync(new PlanItem() {PlanItemID = 1});
            var target = new PlaningController(null, mockPlanItem.Object, null);
            var user = new WebUser() {Id = "1"};

            var result = await target.Edit(1);
            var model = ((PartialViewResult) result).ViewData.Model as EditPlaningModel;

            Assert.AreEqual(model?.PlanItem.PlanItemID,1);
            Assert.IsInstanceOfType(result,typeof(PartialViewResult));
        }

        private async void Edit_UserEditPlaningModelInput_ViewResult()
        {
            var model = new EditPlaningModel();
            var user = new WebUser() {Id = "1"};
            var target = new PlaningController(null,null,null);
            target.ModelState.AddModelError("","");

            var result = await target.Edit(user, model);
            var modelState = ((ViewResult) result).ViewData.ModelState.IsValid;

            Assert.IsFalse(modelState);
            Assert.IsInstanceOfType(result,typeof(ViewResult));
        }

        private async void Edit_UserEditPlaningModelInput_NoModelSpread_RedirectToAction()
        {
            Mock<IPlanItemService> mockPlan = new Mock<IPlanItemService>();       
            var user = new WebUser();
            var model = new EditPlaningModel()
            {
                PlanItem = new PlanItem(),
                Spread = false
            };
            var target = new PlaningController(null,mockPlan.Object,null);
            target.ControllerContext = GetControllerContext(target);

            var result = await target.Edit(user, model);

            mockPlan.Verify(m=>m.UpdateAsync(model.PlanItem));
            mockPlan.Verify(m=>m.SaveAsync());
            Assert.IsInstanceOfType(result,typeof(RedirectToRouteResult));
            Assert.AreEqual(((RedirectToRouteResult)result).RouteValues["action"], "ViewPlan");
        }

        private async void Edit_UserEditPlaningModelInput_ModelSpread_RedirectToAction()
        {
            Mock<IPlanItemService> mockPlan = new Mock<IPlanItemService>();
            Mock<IPlanningHelper> mockHelper = new Mock<IPlanningHelper>();
            var user = new WebUser();
            var model = new EditPlaningModel()
            {
                PlanItem = new PlanItem(),
                Spread = true
            };
            var target = new PlaningController(null, mockPlan.Object, mockHelper.Object);
            target.ControllerContext = GetControllerContext(target);

            var result = await target.Edit(user, model);

            mockHelper.Verify(m=>m.SpreadPlanItems(user, model.PlanItem));
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            Assert.AreEqual(((RedirectToRouteResult)result).RouteValues["action"], "ViewPlan");
        }

        //Проверить данный метод
        private async void Actualize_WebUserInput_RedirectToActionReturned()
        {
            Mock<IPlanningHelper> mock = new Mock<IPlanningHelper>();
            var target = new PlaningController(null,null,mock.Object);

            var result = await target.Actualize(new WebUser() {Id = "1"});
            var model = ((RedirectToRouteResult) result).RouteValues["model"];
            var routeValues = ((RedirectToRouteResult) result).RouteValues;

            Assert.AreEqual(routeValues["action"],"ViewPlan");
        }

        private async Task EditView_IdInput_ViewReturned()
        {
            Mock<IPlanItemService> mock = new Mock<IPlanItemService>();
            var target = new PlaningController(null,mock.Object,null);

            var result = await target.EditView(1);
            var model = ((ViewResult) result).Model;

            Assert.IsNotNull(model);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public async Task PlanItemsTests()
        {
            var user = new WebUser() {Id = "1"};
            await Prepare_UserWithNoCategories_PayingItemListReturned(user);    
            await MethodPrepare_UserWithCategoriesWithNoPlanItems_CreatePlanReturned(user);
            await MethodPrepare_WithCategories_WithPlanItems(user);
            await CreatePlan_RedirectToViewPlanReturned(user);
            await ViewPlan_ViewResultReturned(user);
            Edit_IdInput_PartialViewResulEditPlaningModeltReturned();
            Edit_UserEditPlaningModelInput_ViewResult();
            Edit_UserEditPlaningModelInput_NoModelSpread_RedirectToAction();
            Edit_UserEditPlaningModelInput_ModelSpread_RedirectToAction();
            await EditView_IdInput_ViewReturned();
            Actualize_WebUserInput_RedirectToActionReturned();
        }

    }
}
