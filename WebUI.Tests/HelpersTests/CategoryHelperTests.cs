using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services;
using Moq;
using WebUI.Helpers;
using DomainModels.Model;
using System.Collections.Generic;
using System.Linq;
using Services.Exceptions;
using WebUI.Exceptions;

namespace WebUI.Tests.HelpersTests
{
    [TestClass]
    public class CategoryHelperTests
    {
        private readonly Mock<ICategoryService> _categoryService;
        
        private List<Category> categories => new List<Category>()
        {
            new Category(){ Name = "C1", UserId = "1"},
            new Category(){ Name = "C2", UserId = "2"},
            new Category(){ Name = "C3", UserId = "2"},
            new Category(){ Name = "C4", UserId = "2"},
            new Category(){ Name = "C5", UserId = "1", TypeOfFlowID = 2},
        };

        public CategoryHelperTests()
        {
            _categoryService = new Mock<ICategoryService>();            
        }
        
        [TestCategory("CategoryHelperTests")]
        [TestMethod]
        public async Task GetCategoriesToShowOnPage_CheckByUserId()
        {

            _categoryService.Setup(m => m.GetListAsync()).ReturnsAsync(categories);
            var target = new CategoryHelper(_categoryService.Object);

            var result = (await target.GetCategoriesToShowOnPage(1, 7, CheckByUserId)).ToList();
            
            Assert.AreEqual(2, result.Count());
        }

        [TestCategory("CategoryHelperTests")]
        [TestMethod]
        public async Task GetCategoriesToShowOnPage_CheckByUserIdAndTypeOfFlowId()
        {
            _categoryService.Setup(m => m.GetListAsync()).ReturnsAsync(categories);
            var target = new CategoryHelper(_categoryService.Object);

            var result = (await target.GetCategoriesToShowOnPage(1, 7, CheckByUserIdTypeOfFlowId)).ToList();

            Assert.AreEqual(1, result.Count());
        }

        [TestCategory("CategoryHelperTests")]
        [TestMethod]
        public async Task CreateCategoriesViewModel_UserIdPredicate()
        {
            _categoryService.Setup(m => m.GetListAsync()).ReturnsAsync(categories);
            var target = new CategoryHelper(_categoryService.Object);

            var result = await target.CreateCategoriesViewModel(1, 7, CheckByUserId);
            var pagingInfo = result.PagingInfo;

            Assert.AreEqual(2, result.Categories.Count);
            Assert.IsNotNull(pagingInfo);

        }

        [TestCategory("CategoryHelperTests")]
        [TestMethod]
        public async Task CreateCategoriesViewModel_UserIdTypeOfFlowIdPredicate()
        {
            _categoryService.Setup(m => m.GetListAsync()).ReturnsAsync(categories);
            var target = new CategoryHelper(_categoryService.Object);

            var result = await target.CreateCategoriesViewModel(1, 7, CheckByUserIdTypeOfFlowId);
            var pagingInfo = result.PagingInfo;

            Assert.AreEqual(1, result.Categories.Count);
            Assert.IsNotNull(pagingInfo);

        }

        [TestCategory("CategoryHelperTests")]
        [TestMethod]
        [ExpectedException(typeof(WebUiException))]
        public async Task CreateCategoriesViewModel_ThrowsWebUIException()
        {
            _categoryService.Setup(x => x.GetListAsync()).Throws<WebUiException>();

            var target = new CategoryHelper(_categoryService.Object);
            await target.CreateCategoriesViewModel(1, 1, null);
        }

        [TestCategory("CategoryHelperTests")]
        [TestMethod]
        public async Task CreateCategoriesViewModel_ThrowsWebUiException_WithInnerServcieException()
        {
            try
            {
                _categoryService.Setup(x => x.GetListAsync()).Throws<ServiceException>();

                var target = new CategoryHelper(_categoryService.Object);
                await target.CreateCategoriesViewModel(1, 1, null);
            }
            catch (WebUiException e)
            {
                Assert.IsInstanceOfType(e.InnerException, typeof(ServiceException));
            }
            
        }

        [TestCategory("CategoryHelperTests")]
        [TestMethod]
        [ExpectedException(typeof(WebUiException))]
        public async Task GetCategoriesToShowOnPage_ThrowsWebUiException()
        {
            _categoryService.Setup(x => x.GetListAsync()).Throws<ServiceException>();

            var target = new CategoryHelper(_categoryService.Object);
            await target.GetCategoriesToShowOnPage(1, 1, null);
        }

        [TestCategory("CategoryHelperTests")]
        [TestMethod]
        public async Task GetCategoriesToShowOnPage_ThrowsWebUiException_WithInnerServiceException()
        {
            _categoryService.Setup(x => x.GetListAsync()).Throws<ServiceException>();

            try
            {
                var target = new CategoryHelper(_categoryService.Object);
                await target.GetCategoriesToShowOnPage(1, 1, null);
            }
            catch (WebUiException e)
            {
                Assert.IsInstanceOfType(e.InnerException, typeof(ServiceException));
            }
        }

        private bool CheckByUserId(Category category)
        {
            return category.UserId == "1";
        }

        private bool CheckByUserIdTypeOfFlowId(Category category)
        {
            return category.UserId == "1" && category.TypeOfFlowID == 2;
        }
    }
}
