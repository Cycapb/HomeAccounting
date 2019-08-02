using DomainModels.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Services;
using Services.Exceptions;
using System;
using System.Linq.Expressions;
using WebUI.Exceptions;
using WebUI.Helpers;
using WebUI.Models;

namespace WebUI.Tests.HelpersTests
{
    [TestClass]
    public class ReportControllerHelperTests
    {
        private readonly Mock<ICategoryService> _categoryService;
        private readonly Mock<IPayingItemService> _payingItemService;

        public ReportControllerHelperTests()
        {
            _payingItemService = new Mock<IPayingItemService>();
            _categoryService = new Mock<ICategoryService>();
        }

        [TestMethod]
        [TestCategory("ReportControllerHelperTests")]
        [ExpectedException(typeof(WebUiHelperException))]
        public void GetCategoriesByType_ThrowsWebuiHelperException()
        {
            _categoryService.Setup(m => m.GetList(It.IsAny<Expression<Func<Category,bool>>>())).Throws<ServiceException>();
            var target = new ReportControllerHelper(_categoryService.Object, null, null);

            target.GetCategoriesByType(new WebUser(), 1);
        }

        [TestMethod]
        [TestCategory("ReportControllerHelperTests")]
        public void GetCategoriesByType_ThrowsWebUiHelperExceptionWithInnerServiceException()
        {
            _categoryService.Setup(m => m.GetList()).Throws<ServiceException>();
            var target = new ReportControllerHelper(_categoryService.Object, null, null);

            try
            {
                target.GetCategoriesByType(new WebUser(), 1);
            }
            catch (WebUiHelperException e)
            {
                Assert.IsInstanceOfType(e.InnerException, typeof(ServiceException));
            }
        }

        [TestMethod]
        [TestCategory("ReportControllerHelperTests")]
        [ExpectedException(typeof(WebUiHelperException))]
        public void GetPayingItemsForLastYear_ThrowsWebUiHelperException()
        {            
            _payingItemService.Setup(m => m.GetList(It.IsAny<Expression<Func<PayingItem, bool>>>())).Throws<ServiceException>();
            var target = new ReportControllerHelper(null, _payingItemService.Object, null);

            target.GetPayingItemsForLastYear(new WebUser());
        }
    }
}
