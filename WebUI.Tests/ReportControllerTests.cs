using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DomainModels.Model;
using HomeAccountingSystem_WebUI.Abstract;
using HomeAccountingSystem_WebUI.Controllers;
using HomeAccountingSystem_WebUI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace WebUI.Tests
{
    [TestClass]
    public class ReportControllerTests
    {
        private readonly List<PayingItem> _payingItems = new List<PayingItem>()
        {
            new PayingItem() {ItemID = 1, Summ = 100, Category = new Category() {TypeOfFlowID = 1}},
            new PayingItem() {ItemID = 2, Summ = 100, Category = new Category() {TypeOfFlowID = 1}},
            new PayingItem() {ItemID = 3, Summ = 200, Category = new Category() {TypeOfFlowID = 2}},
            new PayingItem() {ItemID = 4, Summ = 200, Category = new Category() {TypeOfFlowID = 2}}
        };

        private readonly List<OverAllItem> _overAllItems = new List<OverAllItem>()
        {
            new OverAllItem() {Category = "Cat1",Summ = 50M},
            new OverAllItem() {Category = "Cat2",Summ = 100M},
            new OverAllItem() {Category = "Cat3",Summ = 150M},
            new OverAllItem() {Category = "Cat4",Summ = 200M}
        };

        private ControllerContext GetControllerContext(Controller target, bool isAjax)
        {
            Mock<HttpRequestBase> mockHttpRequest = new Mock<HttpRequestBase>();
            if (isAjax)
            {
                mockHttpRequest.SetupGet(x => x.Headers).Returns(new WebHeaderCollection() { "X-Requested-With:XMLHttpRequest" });
            }
            else
            {
                mockHttpRequest.SetupGet(x => x.Headers).Returns(new WebHeaderCollection() { "X-Requested-With:" });
            }
            Mock<HttpContextBase> mockHttpContext = new Mock<HttpContextBase>();
            mockHttpContext.SetupGet(x => x.Request).Returns(mockHttpRequest.Object);
            return new ControllerContext(mockHttpContext.Object, new RouteData(), target);
        }



        [TestMethod]
        [TestCategory("ReportControllerTests")]
        public void OverallLastYearMonths_PartialViewReturned()
        {
            Mock<IReportControllerHelper> mockReportHelper = new Mock<IReportControllerHelper>();
            var target = new ReportController(null, mockReportHelper.Object, null);
            var user = new WebUser() {Id = "1"};

            var result = target.OverallLastYearMonths(user);
            var model = result.ViewData.Model as ReportMonthsModel;

            mockReportHelper.Verify(m=>m.GetPayingItemsForLastYear(user),Times.AtLeastOnce);
            mockReportHelper.Verify(m=>m.FillReportMonthsModel(It.IsAny<ReportMonthsModel>(),It.IsAny<List<PayingItem>>()),Times.AtLeastOnce);
            Assert.IsNotNull(result);
            Assert.IsNotNull(model);
            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
        }

        [TestMethod]
        [TestCategory("ReportControllerTests")]
        public void Index_ViewResultReturned()
        {
            var target = new ReportController(null, null, null);

            var result = target.Index();

            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        [TestCategory("ReportControllerTests")]
        public void CreateByTypeOfFlowView_WebUserIntInput_ViewReturned()
        {
            Mock<IReportControllerHelper> mockReportHelper = new Mock<IReportControllerHelper>();
            var user = new WebUser() {Id = "1"};
            var flowId = 1;

            var target = new ReportController(null, mockReportHelper.Object, null);

            var result = target.CreateByTypeOfFlowView(user, flowId);

            mockReportHelper.Verify(m=>m.GetCategoriesByType(user, flowId),Times.Once);
            Assert.AreEqual(((ViewResult)result).ViewBag.TypeOfFlowId, flowId);
            Assert.IsInstanceOfType(result,typeof(ViewResult));
            Assert.IsNotNull(((ViewResult)result).Model);
        }

        [TestMethod]
        [TestCategory("ReportControllerTests")]
        public void GetByTypeOfFlowReportPartial_PartialViewReportModelReturned()
        {
            Mock<IReportModelCreator> mockCreator = new Mock<IReportModelCreator>();
            mockCreator.Setup(
                m => m.CreateByTypeReportModel(It.IsAny<TempReportModel>(), It.IsAny<WebUser>(), It.IsAny<int>())).Returns(new ReportModel());
            var tempReportModel = new TempReportModel();
            var user = new WebUser() {Id = "1"};
            var page = 1;
            var target = new ReportController(null, null, mockCreator.Object);

            var result = target.GetByTypeOfFlowReportPartial(tempReportModel, user, page);

            mockCreator.Verify(m=>m.CreateByTypeReportModel(tempReportModel, user, page),Times.Once);
            Assert.IsNotNull(((PartialViewResult)result).Model);
            Assert.IsInstanceOfType(result,typeof(PartialViewResult));
        }

        [TestMethod]
        [TestCategory("ReportControllerTests")]
        public void GetTypeOfFlowReport_CatId0_RedrectToRouteResultReturned()
        {
            var tempReportModel = new TempReportModel() {CatId = 0, TypeOfFlowId = 1};
            var user = new WebUser() {Id = "1"};
            var page = 1;
            var target = new ReportController(null, null, null);

            var result = target.GetTypeOfFlowReport(tempReportModel, user, page);

            Assert.IsTrue(target.TempData.Keys.Count == 1);
            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
        }

        [TestMethod]
        [TestCategory("ReportControllerTests")]
        public void GetTypeOfFlowReport_CatIdNot0_ViewResultReturned()
        {
            Mock<IReportModelCreator> mockCreator = new Mock<IReportModelCreator>();
            mockCreator.Setup(
                m => m.CreateByTypeReportModel(It.IsAny<TempReportModel>(), It.IsAny<WebUser>(), It.IsAny<int>()))
                .Returns(new ReportModel());
            var target = new ReportController(null, null, mockCreator.Object);
            var tempReportModel = new TempReportModel() {CatId = 1};
            var page = 1;
            var user = new WebUser() {Id = "1"};

            var result = target.GetTypeOfFlowReport(tempReportModel, user, page);
            var model = ((PartialViewResult) result).Model;

            mockCreator.Verify(m=>m.CreateByTypeReportModel(tempReportModel,user,page),Times.AtLeastOnce);
            Assert.IsNotNull(model);
            Assert.IsInstanceOfType(result,typeof(PartialViewResult));
        }

        [TestMethod]
        [TestCategory("ReportControllerTests")]
        public void CreateByDatesView_ViewReturned()
        {
            var target = new ReportController(null, null, null);

            var result = target.CreateByDatesView();
            
            Assert.IsInstanceOfType(result,typeof(ViewResult));
        }

        [TestMethod]
        [TestCategory("ReportControllerTests")]
        public void GetByDatesReportView_ViewResultReturned()
        {
            Mock<IReportControllerHelper> mockHelper = new Mock<IReportControllerHelper>();   
            Mock<IReportModelCreator> mockCreator = new Mock<IReportModelCreator>();
            mockHelper.Setup(
                m => m.GetPayingItemsInDates(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<WebUser>()))
                .Returns(_payingItems);
            mockCreator.Setup(
                m =>
                    m.CreateByDatesReportModel(It.IsAny<WebUser>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                        It.IsAny<int>())).Returns(new ReportModel());
            var target = new ReportController(null, mockHelper.Object, mockCreator.Object);
            var user = new WebUser();

            var result = target.GetByDatesReportView(user, DateTime.Today, DateTime.Today, 1 );

            mockHelper.Verify(m=>m.GetPayingItemsInDates(DateTime.Today, DateTime.Today, user),Times.Once);
            mockCreator.Verify(m=>m.CreateByDatesReportModel(user, DateTime.Today, DateTime.Today,1));
            Assert.IsInstanceOfType(result,typeof(ViewResult));
            Assert.IsNotNull(((ViewResult)result).Model);
            Assert.AreEqual(((ViewResult)result).ViewBag.OutgoSum,400);
            Assert.AreEqual(((ViewResult)result).ViewBag.IncomingSum, 200);
        }

        [TestMethod]
        [TestCategory("ReportControllerTests")]
        public void GetByDatesReportView_PartialViewResultReturned()
        {
            Mock<IReportControllerHelper> mockHelper = new Mock<IReportControllerHelper>();
            Mock<IReportModelCreator> mockCreator = new Mock<IReportModelCreator>();
            mockHelper.Setup(
                m => m.GetPayingItemsInDates(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<WebUser>()))
                .Returns(_payingItems);
            mockCreator.Setup(
                m =>
                    m.CreateByDatesReportModel(It.IsAny<WebUser>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                        It.IsAny<int>())).Returns(new ReportModel());
            var target = new ReportController(null, mockHelper.Object, mockCreator.Object);
            var user = new WebUser();

            var result = target.GetByDatesReport(user, DateTime.Today, DateTime.Today, 1);

            mockHelper.Verify(m => m.GetPayingItemsInDates(DateTime.Today, DateTime.Today, user), Times.Once);
            mockCreator.Verify(m => m.CreateByDatesReportModel(user, DateTime.Today, DateTime.Today, 1));
            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
            Assert.IsNotNull(((PartialViewResult)result).Model);
            Assert.AreEqual(((PartialViewResult)result).ViewBag.OutgoSum, 400);
            Assert.AreEqual(((PartialViewResult)result).ViewBag.IncomingSum, 200);
        }

        [TestMethod]
        [TestCategory("ReportControllerTests")]
        public void GetByDatesReportPartial_PartialReportModeltReturned()
        {
            Mock<IReportModelCreator> mockCreator = new Mock<IReportModelCreator>();
            mockCreator.Setup(
               m =>
                   m.CreateByDatesReportModel(It.IsAny<WebUser>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                       It.IsAny<int>())).Returns(new ReportModel());
            var target = new ReportController(null,null,mockCreator.Object);

            var result = target.GetByDatesReportPartial(new WebUser(), DateTime.Today, DateTime.Today, 1);
            var model = ((PartialViewResult) result).Model;

            Assert.IsInstanceOfType(result,typeof(PartialViewResult));
            Assert.IsNotNull(model);
        }

        [TestMethod]
        [TestCategory("ReportControllerTests")]
        public void GetAllCategoriesReport_PartialOverAllItemListReturned()
        {
            Mock<IReportControllerHelper> mockHelper = new Mock<IReportControllerHelper>();
            mockHelper.Setup(
                m => m.GetOverallList(It.IsAny<WebUser>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>()))
                .Returns(_overAllItems);
            var target = new ReportController(null,mockHelper.Object,null);

            var result = target.GetAllCategoriesReport(new WebUser(), DateTime.Today, DateTime.Today, 1);
            var viewBag = ((PartialViewResult) result).ViewBag;

            Assert.AreEqual(viewBag.TypeOfFlowName,"Доход");
            Assert.AreEqual(viewBag.dtFrom, DateTime.Today);
            Assert.AreEqual(viewBag.dtTo, DateTime.Today);
            Assert.AreEqual(viewBag.Summ,500);
            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
            Assert.IsNotNull(((PartialViewResult)result).Model);
        }

        [TestMethod]
        [TestCategory("ReportControllerTests")]
        public void GetItemsByMonth_RedirectToGetByDatesReportReturned()
        {
            var user = new WebUser();
            var target = new ReportController(null,null,null);
            target.ControllerContext = GetControllerContext(target,false);

            var result = target.GetItemsByMonth(user,DateTime.Today);

            Assert.IsInstanceOfType(result,typeof(RedirectToRouteResult));
            Assert.AreEqual(((RedirectToRouteResult)result).RouteValues["action"], "GetByDatesReportView");
            Assert.AreEqual(((RedirectToRouteResult)result).RouteValues["dtFrom"],DateTime.Today);
            Assert.IsNotNull(((RedirectToRouteResult)result).RouteValues["dtTo"]);
        }

        [TestMethod]
        [TestCategory("ReportControllerTests")]
        public void GetItemsByMonth_AjaxRedirectToGetByDatesReportReturned()
        {
            var user = new WebUser();
            var target = new ReportController(null, null, null);
            target.ControllerContext = GetControllerContext(target, true);

            var result = target.GetItemsByMonth(user, DateTime.Today);

            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            Assert.AreEqual(((RedirectToRouteResult)result).RouteValues["action"], "GetByDatesReport");
            Assert.AreEqual(((RedirectToRouteResult)result).RouteValues["dtFrom"], DateTime.Today);
            Assert.IsNotNull(((RedirectToRouteResult)result).RouteValues["dtTo"]);
        }

        [TestMethod]
        [TestCategory("ReportControllerTests")]
        public async Task SubcategoriesReportView_WebUserIntDateTimeInput_ViewReturned()
        {
            Mock<IPayItemSubcategoriesHelper> mockHelper = new Mock<IPayItemSubcategoriesHelper>();
            mockHelper.Setup(
                m =>
                    m.GetPayItemsWithSubcategoriesInDatesWeb(It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                        It.IsAny<WebUser>(), It.IsAny<int>())).ReturnsAsync(new List<PayItemSubcategories>());
            var target = new ReportController(mockHelper.Object, null, null);

            var result = await target.SubcategoriesReportView(new WebUser(), 1, DateTime.Today);
            var model = ((ViewResult)result).Model;

            mockHelper.Verify(m => m.GetPayItemsWithSubcategoriesInDatesWeb(It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                        It.IsAny<WebUser>(), It.IsAny<int>()), Times.Once);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsNotNull(model);
            Assert.AreEqual(((ViewResult)result).ViewBag.TypeOfFlowName, "Доход");
            Assert.AreEqual(((ViewResult)result).ViewBag.Summ, 0);
            Assert.AreEqual(((ViewResult)result).ViewBag.Month, DateTime.Today.Date.ToString("MMMMM"));
        }

        [TestMethod]
        [TestCategory("ReportControllerTests")]
        public async Task SubcategoriesReport_WebUserIntDateTimeInput_PartialViewReturned()
        {
            Mock<IPayItemSubcategoriesHelper> mockHelper = new Mock<IPayItemSubcategoriesHelper>();
            mockHelper.Setup(
                m =>
                    m.GetPayItemsWithSubcategoriesInDatesWeb(It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                        It.IsAny<WebUser>(), It.IsAny<int>())).ReturnsAsync(new List<PayItemSubcategories>());
            var target = new ReportController(mockHelper.Object, null, null);

            var result = await target.SubcategoriesReport(new WebUser(), 1, DateTime.Today);
            var model = ((PartialViewResult)result).Model;

            mockHelper.Verify(m => m.GetPayItemsWithSubcategoriesInDatesWeb(It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                        It.IsAny<WebUser>(), It.IsAny<int>()), Times.Once);
            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
            Assert.IsNotNull(model);
            Assert.AreEqual(((PartialViewResult)result).ViewBag.TypeOfFlowName, "Доход");
            Assert.AreEqual(((PartialViewResult)result).ViewBag.Summ, 0);
            Assert.AreEqual(((PartialViewResult)result).ViewBag.Month, DateTime.Today.Date.ToString("MMMMM"));
        }
    }
}
