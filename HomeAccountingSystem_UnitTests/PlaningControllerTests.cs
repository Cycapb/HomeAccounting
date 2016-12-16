using System.Collections.Generic;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using HomeAccountingSystem_DAL.Abstract;
using HomeAccountingSystem_DAL.Model;
using HomeAccountingSystem_DAL.Repositories;
using HomeAccountingSystem_WebUI.Abstract;
using HomeAccountingSystem_WebUI.Controllers;
using HomeAccountingSystem_WebUI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

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

        private void Prepare_UserWithNoCategories_PayingItemListReturned(WebUser user)
        {
            Mock<IRepository<Category>> mockCategory = new Mock<IRepository<Category>>();
            Mock<IRepository<PlanItem>> mockPlanItem = new Mock<IRepository<PlanItem>>();
            mockPlanItem.Setup(m => m.GetList()).Returns(new List<PlanItem>());
            mockCategory.Setup(m => m.GetList()).Returns(new List<Category>());
            var target = new PlaningController(mockCategory.Object,mockPlanItem.Object, null,null);
            
            var result = target.Prepare(user);

            var controller = result.RouteValues["controller"];
            var action = result.RouteValues["action"];

            Assert.AreEqual("Index",action);
            Assert.AreEqual("PayingItem",controller);
        }

        private void MethodPrepare_UserWithCategoriesWithNoPlanItems_CreatePlanReturned(WebUser user)
        {
            Mock<IRepository<Category>> mockCategory = new Mock<IRepository<Category>>();
            Mock<IRepository<PlanItem>> mockPlanItem = new Mock<IRepository<PlanItem>>();
            mockCategory.Setup(m => m.GetList()).Returns(_categories);
            var target = new PlaningController(mockCategory.Object, mockPlanItem.Object, null,null);

            var result = target.Prepare(user);
            var controller = result.RouteValues["controller"];
            var action = result.RouteValues["action"];

            Assert.IsNull(controller);
            Assert.AreEqual("CreatePlan",action);
        }

        private void MethodPrepare_WithCategories_WithPlanItems(WebUser user)
        {
            Mock<IRepository<Category>> mockCategory = new Mock<IRepository<Category>>();
            Mock<IRepository<PlanItem>> mockPlanItem = new Mock<IRepository<PlanItem>>();
            mockCategory.Setup(m => m.GetList()).Returns(_categories);
            mockPlanItem.Setup(m => m.GetList()).Returns(_planItems);
            var target = new PlaningController(mockCategory.Object, mockPlanItem.Object, null,null);

            var result = target.Prepare(user);
            var controller = result.RouteValues["controller"];
            var action = result.RouteValues["action"];

            Assert.IsNull(controller);
            Assert.AreEqual("ViewPlan", action);
        }

        private async void CreatePlan_RedirectToViewPlanReturned(WebUser user)
        {
            Mock<IPlanningHelper> helperMock = new Mock<IPlanningHelper>();
            Mock<IRepository<PlanItem>> mockPlanItem = new Mock<IRepository<PlanItem>>();
            Mock<IRepository<Category>> mockCategory = new Mock<IRepository<Category>>();
            Mock<IRepository<PayingItem>> mockPayingItem = new Mock<IRepository<PayingItem>>();
            var target = new PlaningController(mockCategory.Object,mockPlanItem.Object,mockPayingItem.Object,helperMock.Object);

            var result = await target.CreatePlan(user);

            helperMock.Verify(m=>m.CreatePlanItems(user));
            Assert.AreEqual(result.RouteValues["action"],"ViewPlan");
        }

        private void ViewPlan_ViewResultReturned(WebUser user)
        {
            Mock<IPlanningHelper> helperMock = new Mock<IPlanningHelper>();
            Mock<IRepository<PlanItem>> mockPlanItem = new Mock<IRepository<PlanItem>>();
            Mock<IRepository<Category>> mockCategory = new Mock<IRepository<Category>>();
            Mock<IRepository<PayingItem>> mockPayingItem = new Mock<IRepository<PayingItem>>();
            helperMock.Setup(m => m.GetUserBalance(It.IsAny<IWorkingUser>(),It.IsAny<bool>())).ReturnsAsync(new ViewPlaningModel());
            var target = new PlaningController(mockCategory.Object,mockPlanItem.Object,mockPayingItem.Object,helperMock.Object);

            var result = target.ViewPlan(user);
            
            Assert.IsNotNull(target.ViewData.Model);
            Assert.IsInstanceOfType(result,typeof(ViewResult));
        }

        private async void Edit_IdInput_PartialViewResulEditPlaningModeltReturned()
        {
            Mock<IRepository<PlanItem>> mockPlanItem = new Mock<IRepository<PlanItem>>();
            mockPlanItem.Setup(m => m.GetItemAsync(It.IsAny<int>())).ReturnsAsync(new PlanItem() {PlanItemID = 1});
            var target = new PlaningController(null, mockPlanItem.Object, null, null);
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
            var target = new PlaningController(null,null,null,null);
            target.ModelState.AddModelError("","");

            var result = await target.Edit(user, model);
            var modelState = ((ViewResult) result).ViewData.ModelState.IsValid;

            Assert.IsFalse(modelState);
            Assert.IsInstanceOfType(result,typeof(ViewResult));
        }

        private async void Edit_UserEditPlaningModelInput_NoModelSpread_RedirectToAction()
        {
            Mock<IRepository<PlanItem>> mockPlan = new Mock<IRepository<PlanItem>>();       
            var user = new WebUser();
            var model = new EditPlaningModel()
            {
                PlanItem = new PlanItem(),
                Spread = false
            };
            var target = new PlaningController(null,mockPlan.Object,null,null);
            target.ControllerContext = GetControllerContext(target);

            var result = await target.Edit(user, model);

            mockPlan.Verify(m=>m.UpdateAsync(model.PlanItem));
            mockPlan.Verify(m=>m.SaveAsync());
            Assert.IsInstanceOfType(result,typeof(RedirectToRouteResult));
            Assert.AreEqual(((RedirectToRouteResult)result).RouteValues["action"], "ViewPlan");
        }

        private async void Edit_UserEditPlaningModelInput_ModelSpread_RedirectToAction()
        {
            Mock<IRepository<PlanItem>> mockPlan = new Mock<IRepository<PlanItem>>();
            Mock<IPlanningHelper> mockHelper = new Mock<IPlanningHelper>();
            var user = new WebUser();
            var model = new EditPlaningModel()
            {
                PlanItem = new PlanItem(),
                Spread = true
            };
            var target = new PlaningController(null, mockPlan.Object, null, mockHelper.Object);
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
            var target = new PlaningController(null,null,null,mock.Object);

            var result = await target.Actualize(new WebUser() {Id = "1"});
            var model = ((RedirectToRouteResult) result).RouteValues["model"];
            var routeValues = ((RedirectToRouteResult) result).RouteValues;

            Assert.AreEqual(routeValues["action"],"ViewPlan");
        }

        private async void EditView_IdInput_ViewReturned()
        {
            Mock<IRepository<PlanItem>> mock = new Mock<IRepository<PlanItem>>();
            var target = new PlaningController(null,mock.Object,null,null);

            var result = await target.EditView(1);
            var model = ((ViewResult) result).Model;

            Assert.IsNotNull(model);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void PlanItemsTests()
        {
            var user = new WebUser() {Id = "1"};
            Prepare_UserWithNoCategories_PayingItemListReturned(user);    
            MethodPrepare_UserWithCategoriesWithNoPlanItems_CreatePlanReturned(user);
            MethodPrepare_WithCategories_WithPlanItems(user);
            CreatePlan_RedirectToViewPlanReturned(user);
            ViewPlan_ViewResultReturned(user);
            Edit_IdInput_PartialViewResulEditPlaningModeltReturned();
            Edit_UserEditPlaningModelInput_ViewResult();
            Edit_UserEditPlaningModelInput_NoModelSpread_RedirectToAction();
            Edit_UserEditPlaningModelInput_ModelSpread_RedirectToAction();
            EditView_IdInput_ViewReturned();
            Actualize_WebUserInput_RedirectToActionReturned();
        }

    }
}
