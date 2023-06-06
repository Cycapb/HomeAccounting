using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Paginator.Abstract;
using WebUI.Core.Abstract;
using WebUI.Core.Abstract.Helpers;
using WebUI.Core.Controllers;
using WebUI.Core.Exceptions;
using WebUI.Core.Models;
using WebUI.Core.Models.CategoryModels;
using WebUI.Core.Models.ReportModels;

namespace WebUI.Core.Tests.Controllers
{
    [TestClass]
    public class ReportControllerTests
    {
        private readonly List<CategorySumModel> _overAllItems = new()
        {
            new CategorySumModel {Category = "Cat1",Sum = 50M},
            new CategorySumModel {Category = "Cat2",Sum = 100M},
            new CategorySumModel {Category = "Cat3",Sum = 150M},
            new CategorySumModel {Category = "Cat4",Sum = 200M}
        };
        private readonly Mock<IReportModelCreator> _reportModelCreator;
        private readonly Mock<IReportControllerHelper> _reportControllerHelperMock;
        private readonly Mock<IPayItemSubcategoriesHelper> _payItemSubcategoriesHelperMock;
        private readonly Mock<IPageCreator> _pageCreator;

        public ReportControllerTests()
        {
            _reportModelCreator = new Mock<IReportModelCreator>();
            _reportControllerHelperMock = new Mock<IReportControllerHelper>();
            _payItemSubcategoriesHelperMock = new Mock<IPayItemSubcategoriesHelper>();
            _pageCreator = new Mock<IPageCreator>();
        }

        [TestMethod]
        [TestCategory("ReportControllerTests")]
        public void Index_ViewResultReturned()
        {
            var target = new ReportController(null, null, null, null);

            var result = target.Index();

            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
        }

        [TestMethod]
        [TestCategory("ReportControllerTests")]
        public void CreateByTypeOfFlowView_WebUserIntInput_ViewReturned()
        {
            var mockReportHelper = new Mock<IReportControllerHelper>();
            var user = new WebUser {Id = "1"};
            var flowId = 1;

            var target = new ReportController(null, mockReportHelper.Object, null, null);

            var result = target.CreateByTypeOfFlowView(user, flowId);

            mockReportHelper.Verify(m=>m.GetActiveCategoriesByType(user, flowId),Times.Once);
            Assert.AreEqual(flowId, ((PartialViewResult)result).ViewData["TypeOfFlowId"]);
            Assert.IsInstanceOfType(result,typeof(PartialViewResult));
            Assert.IsNotNull(((PartialViewResult)result).Model);
        }

        [TestMethod]
        [TestCategory("ReportControllerTests")]
        public void GetByTypeOfFlowReportPartial_PartialViewReportModelReturned()
        {
            var mockCreator = new Mock<IReportModelCreator>();
            mockCreator.Setup(
                m => m.CreateByTypeReportModel(It.IsAny<ReportByCategoryAndTypeOfFlowModel>(), It.IsAny<WebUser>(), It.IsAny<int>())).Returns(new ReportModel());
            var tempReportModel = new ReportByCategoryAndTypeOfFlowModel();
            var user = new WebUser {Id = "1"};
            var page = 1;
            var target = new ReportController(null, null, mockCreator.Object, null);

            var result = target.GetTypeOfFlowReport(tempReportModel, user, page);

            mockCreator.Verify(m=>m.CreateByTypeReportModel(tempReportModel, user, page),Times.Once);
            Assert.IsNotNull(((PartialViewResult)result).Model);
            Assert.IsInstanceOfType(result,typeof(PartialViewResult));
        }

        [TestMethod]
        [TestCategory("ReportControllerTests")]
        public void GetTypeOfFlowReport_CatId0_RedirectToRouteResultReturned()
        {
            var target = new ReportController(null, null, _reportModelCreator.Object, null);
            _reportModelCreator.Setup(m => m.CreateByTypeReportModel(It.IsAny<ReportByCategoryAndTypeOfFlowModel>(), It.IsAny<WebUser>(), It.IsAny<int>())).Returns(new ReportModel());

            var result = target.GetTypeOfFlowReport(new ReportByCategoryAndTypeOfFlowModel(), new WebUser());
            var model = ((PartialViewResult)result).Model;

            Assert.IsNotNull(model);
            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
        }

        [TestMethod]
        [TestCategory("ReportControllerTests")]
        public void GetTypeOfFlowReport_CatIdNot0_ViewResultReturned()
        {
            Mock<IReportModelCreator> mockCreator = new Mock<IReportModelCreator>();
            mockCreator.Setup(
                m => m.CreateByTypeReportModel(It.IsAny<ReportByCategoryAndTypeOfFlowModel>(), It.IsAny<WebUser>(), It.IsAny<int>()))
                .Returns(new ReportModel());
            var target = new ReportController(null, null, mockCreator.Object, null);
            var tempReportModel = new ReportByCategoryAndTypeOfFlowModel { CategoryId = 1 };
            var page = 1;
            var user = new WebUser {Id = "1"};

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
            var target = new ReportController(null, null, null, null);

            var result = target.CreateByDatesView();
            
            Assert.IsInstanceOfType(result,typeof(PartialViewResult));
        }

        [TestMethod]
        [TestCategory("ReportControllerTests")]
        public void GetByDatesReportView_ViewResultReturned()
        {            
            Mock<IReportModelCreator> mockCreator = new Mock<IReportModelCreator>();            
            mockCreator.Setup(
                m =>
                    m.CreateByDatesReportModel(It.IsAny<WebUser>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                        It.IsAny<int>())).Returns(new ReportModel
            { AllItems = new List<PayItem>
                {
                            new()
                            {
                                TypeOfFlowId = 1,
                                Summ = 200
                            },
                            new()
                            {
                                TypeOfFlowId = 2,
                                Summ = 400
                            }
                        }
                        });
            var target = new ReportController(null, null, mockCreator.Object, null);
            var user = new WebUser();

            var result = target.GetByDatesReport(user, DateTime.Today, DateTime.Today );
                        
            mockCreator.Verify(m=>m.CreateByDatesReportModel(user, DateTime.Today, DateTime.Today,1));
            Assert.IsInstanceOfType(result,typeof(PartialViewResult));
            Assert.IsNotNull(((PartialViewResult)result).Model);
            Assert.AreEqual(400M, ((PartialViewResult)result).ViewData["OutgoSum"]);
            Assert.AreEqual(200M, ((PartialViewResult)result).ViewData["IncomingSum"]);
        }

        [TestMethod]
        [TestCategory("ReportControllerTests")]
        public void GetByDatesReportView_PartialViewResultReturned()
        {
            Mock<IReportModelCreator> mockCreator = new Mock<IReportModelCreator>();
            mockCreator.Setup(
                m =>
                    m.CreateByDatesReportModel(It.IsAny<WebUser>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                        It.IsAny<int>())).Returns(new ReportModel
            { AllItems = new List<PayItem>
                {
                            new()
                            {
                                TypeOfFlowId = 1,
                                Summ = 200
                            },
                            new()
                            {
                                TypeOfFlowId = 2,
                                Summ = 400
                            }
                        }
                        });
            var target = new ReportController(null, null, mockCreator.Object, null);
            var user = new WebUser();

            var result = target.GetByDatesReport(user, DateTime.Today, DateTime.Today);
                        
            mockCreator.Verify(m => m.CreateByDatesReportModel(user, DateTime.Today, DateTime.Today, 1));
            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
            Assert.IsNotNull(((PartialViewResult)result).Model);
            Assert.AreEqual(400M, ((PartialViewResult)result).ViewData["OutgoSum"]);
            Assert.AreEqual(200M, ((PartialViewResult)result).ViewData["IncomingSum"]);
        }

        [TestMethod]
        [TestCategory("ReportControllerTests")]
        public void GetByDatesReportPartial_PartialReportModeltReturned()
        {
            Mock<IReportModelCreator> mockCreator = new Mock<IReportModelCreator>();
            mockCreator.Setup(
               m =>
                   m.CreateByDatesReportModel(It.IsAny<WebUser>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                       It.IsAny<int>())).Returns(new ReportModel { AllItems = new List<PayItem>()});
            var target = new ReportController(null,_reportControllerHelperMock.Object,mockCreator.Object, _pageCreator.Object);

            var result = target.GetByDatesReport(new WebUser(), DateTime.Today, DateTime.Today);
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
            var target = new ReportController(null,mockHelper.Object,null,null);

            var result = target.GetAllCategoriesReport(new WebUser(), DateTime.Today, DateTime.Today, 1);
            var viewBag = ((PartialViewResult) result).ViewData;

            Assert.AreEqual("Доход", viewBag["TypeOfFlowName"]);
            Assert.AreEqual(DateTime.Today, viewBag["dtFrom"]);
            Assert.AreEqual(DateTime.Today, viewBag["dtTo"]);
            Assert.AreEqual(500M, viewBag["Summ"]);
            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
            Assert.IsNotNull(((PartialViewResult)result).Model);
        }

        [TestMethod]
        [TestCategory("ReportControllerTests")]
        public void GetItemsByMonth_RedirectToGetByDatesReportReturned()
        {
            var target = new ReportController(null,null,null,null);

            var result = target.GetItemsByMonth(DateTime.Today);

            Assert.IsInstanceOfType(result,typeof(RedirectToActionResult));
            Assert.AreEqual("GetByDatesReport", ((RedirectToActionResult)result).ActionName);
            Assert.AreEqual(DateTime.Today, ((RedirectToActionResult)result).RouteValues!["dtFrom"]);
            Assert.IsNotNull(((RedirectToActionResult)result).RouteValues!["dtTo"]);
        }

        [TestMethod]
        [TestCategory("ReportControllerTests")]
        public void GetItemsByMonth_AjaxRedirectToGetByDatesReportReturned()
        {
            var target = new ReportController(null, null, null,null);

            var result = target.GetItemsByMonth(DateTime.Today);

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            Assert.AreEqual("GetByDatesReport", ((RedirectToActionResult)result).ActionName);
            Assert.AreEqual(DateTime.Today, ((RedirectToActionResult)result).RouteValues!["dtFrom"]);
            Assert.IsNotNull(((RedirectToActionResult)result).RouteValues!["dtTo"]);
        }

        [TestMethod]
        [TestCategory("ReportControllerTests")]
        public async Task SubcategoriesReport_WebUserIntDateTimeInput_PartialViewReturned()
        {
            var mockHelper = new Mock<IPayItemSubcategoriesHelper>();
            mockHelper.Setup(
                m =>
                    m.GetPayItemsWithSubcategoriesInDatesWeb(It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                        It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(new List<PayItemSubcategories>());
            var target = new ReportController(mockHelper.Object, null, null,null);

            var result = await target.SubcategoriesReport(new WebUser(), 1, DateTime.Today);
            var model = ((PartialViewResult)result).Model;

            mockHelper.Verify(m => m.GetPayItemsWithSubcategoriesInDatesWeb(It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                        It.IsAny<string>(), It.IsAny<int>()), Times.Once);
            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
            Assert.IsNotNull(model);
            Assert.AreEqual("Доход", ((PartialViewResult)result).ViewData["TypeOfFlowName"]);
            Assert.AreEqual(0M, ((PartialViewResult)result).ViewData["Summ"]);
            Assert.AreEqual(DateTime.Today.Date.ToString("MMMMM"), ((PartialViewResult)result).ViewData["Month"]);
        }

        [TestMethod]
        [TestCategory("ReportControllerTests")]
        [ExpectedException(typeof(WebUiException))]
        public void GetTypeOfFlowReport_RaisesWebUiException()
        {
            _reportModelCreator
                .Setup(m => m.CreateByTypeReportModel(It.IsAny<ReportByCategoryAndTypeOfFlowModel>(), It.IsAny<WebUser>(), It.IsAny<int>()))
                .Throws<WebUiException>();
            var target = new ReportController(null, null, _reportModelCreator.Object,null);

            target.GetTypeOfFlowReport(new ReportByCategoryAndTypeOfFlowModel(), new WebUser());
        }

        [TestMethod]
        [TestCategory("ReportControllerTests")]
        public void GetTypeOfFlowReport_RaisesWebUiExceptionWithInnerWebUihelperException()
        {
            _reportModelCreator
                .Setup(m => m.CreateByTypeReportModel(It.IsAny<ReportByCategoryAndTypeOfFlowModel>(), It.IsAny<WebUser>(), It.IsAny<int>()))
                .Throws<WebUiException>();
            var target = new ReportController(null, null, _reportModelCreator.Object,null);

            try
            {
                target.GetTypeOfFlowReport(It.IsAny<ReportByCategoryAndTypeOfFlowModel>(), It.IsAny<WebUser>(), It.IsAny<int>());
            }
            catch (WebUiException e)
            {
                Assert.IsInstanceOfType(e.InnerException, typeof(WebUiException));
            }
        }

        [TestMethod]
        [TestCategory("ReportControllerTests")]
        [ExpectedException(typeof(WebUiException))]
        public void GetByDatesReport_RaisesWebuiException()
        {
            _reportModelCreator
                .Setup(m => m.CreateByDatesReportModel(It.IsAny<WebUser>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>()))
                .Throws<WebUiException>();
            var target = new ReportController(null, null, _reportModelCreator.Object,null);

            target.GetByDatesReport(new WebUser(), DateTime.Now, DateTime.Now);
        }

        [TestMethod]
        [TestCategory("ReportControllerTests")]
        public void GetByDatesReport_RaisesWebuiExceptionWithInnerWebUiHelperException()
        {
            _reportModelCreator
                .Setup(m => m.CreateByDatesReportModel(It.IsAny<WebUser>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>()))
                .Throws<WebUiException>();
            var target = new ReportController(null, null, _reportModelCreator.Object, null);

            try
            {
                target.GetByDatesReport(new WebUser(), DateTime.Now, DateTime.Now);
            }
            catch (WebUiException e)
            {
                Assert.IsInstanceOfType(e.InnerException, typeof(WebUiException));
            }
        }

        [TestMethod]
        [TestCategory("ReportControllerTests")]
        [ExpectedException(typeof(WebUiException))]
        public async Task SubcategoriesReport_RaisesWebUiException()
        {
            _payItemSubcategoriesHelperMock
                .Setup(m => m.GetPayItemsWithSubcategoriesInDatesWeb(It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                    It.IsAny<string>(), It.IsAny<int>())).Throws<WebUiHelperException>();
            var target = new ReportController(_payItemSubcategoriesHelperMock.Object, null, null,null);

            await target.SubcategoriesReport(new WebUser(), 1, DateTime.Now);
        }

        [TestMethod]
        [TestCategory("ReportControllerTests")]
        public async Task SubcategoriesReport_RaisesWebUiExceptionWithInnerWebUiHelperException()
        {
            _payItemSubcategoriesHelperMock
                .Setup(m => m.GetPayItemsWithSubcategoriesInDatesWeb(It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                    It.IsAny<string>(), It.IsAny<int>())).Throws<WebUiHelperException>();
            var target = new ReportController(_payItemSubcategoriesHelperMock.Object, null, null,null);

            try
            {
                await target.SubcategoriesReport(new WebUser(), 1, DateTime.Now);
            }
            catch (WebUiException e)
            {
                Assert.IsInstanceOfType(e.InnerException, typeof(WebUiHelperException));
            }
        }
    }
}
