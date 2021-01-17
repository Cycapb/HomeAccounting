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
using WebUI.Core.Exceptions;
using WebUI.Core.Implementations.Helpers;

namespace WebUI.Tests.HelpersTests
{
    [TestClass]
    public class CategoryHelperTests
    {
        private readonly Mock<ICategoryService> _categoryService;

        private List<Category> Categories => new List<Category>()
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
        public async Task GetCategoriesToShowOnPageAsync_Input_Page1ItemsPerPage7FilteredByUserId_ReturnsTwoCategoriesForUser()
        {
            var category = new Category() { UserId = "1" };
            _categoryService.Setup(m => m.GetListAsync(It.Is<Expression<Func<Category, bool>>>(x => x.Compile()(category))))
                .ReturnsAsync(Categories.Where(category => category.UserId == "1"));
            var target = new CategoryHelper(_categoryService.Object);
            var page = 1;
            var itemsPerPage = 7;

            var result = (await target.GetCategoriesToShowOnPageAsync(page, itemsPerPage, category => category.UserId == "1")).ToList();

            Assert.AreEqual(2, result.Count);
        }

        [TestCategory("CategoryHelperTests")]
        [TestMethod]
        public async Task GetCategoriesToShowOnPageAsync_Input_Page1ItemsPerPage7FilteredByUserIdAndTypeOfFlowId_ReturnsOneCategoryForUser()
        {
            var category = new Category() { UserId = "1", TypeOfFlowID = 2 };
            _categoryService.Setup(m => m.GetListAsync(It.Is<Expression<Func<Category, bool>>>(x => x.Compile()(category))))
                .ReturnsAsync(Categories.Where(category => category.UserId == "1" && category.TypeOfFlowID == 2));
            var target = new CategoryHelper(_categoryService.Object);
            var page = 1;
            var itemsPerPage = 7;

            var result = (await target.GetCategoriesToShowOnPageAsync(page, itemsPerPage, category => category.UserId == "1" && category.TypeOfFlowID == 2)).ToList();

            Assert.AreEqual(1, result.Count);
        }

        [TestCategory("CategoryHelperTests")]
        [TestMethod]
        public async Task CreateCategoriesViewModelAsync_Input_Page1ItemsPerPage7FilteredByUserId_Returns2CategoriesForUser()
        {
            var category = new Category() { UserId = "1", TypeOfFlowID = 2 };
            _categoryService.Setup(m => m.GetListAsync(It.Is<Expression<Func<Category, bool>>>(x => x.Compile()(category))))
                .ReturnsAsync(Categories.Where(category => category.UserId == "1"));
            var target = new CategoryHelper(_categoryService.Object);
            var page = 1;
            var itemsPerPage = 7;

            var result = await target.CreateCategoriesViewModelAsync(page, itemsPerPage, category => category.UserId == "1");

            Assert.AreEqual(2, result.Categories.Count);
            Assert.IsNotNull(result.PagingInfo);

        }

        [TestCategory("CategoryHelperTests")]
        [TestMethod]
        public async Task CreateCategoriesViewModelAsync_Input_Page1ItemsPerPage7FilteredByUserIdAndTypeOfFlowId_Returns1CategoryForUser()
        {
            var category = new Category() { UserId = "1", TypeOfFlowID = 2 };
            _categoryService.Setup(m => m.GetListAsync(It.Is<Expression<Func<Category, bool>>>(x => x.Compile()(category))))
                .ReturnsAsync(Categories.Where(category => category.UserId == "1" && category.TypeOfFlowID == 2));
            var target = new CategoryHelper(_categoryService.Object);
            var page = 1;
            var itemsPerPage = 7;

            var result = await target.CreateCategoriesViewModelAsync(page, itemsPerPage, category => category.UserId == "1" && category.TypeOfFlowID == 2);

            Assert.AreEqual(1, result.Categories.Count);
            Assert.IsNotNull(result.PagingInfo);

        }

        [TestCategory("CategoryHelperTests")]
        [TestMethod]
        [ExpectedException(typeof(WebUiHelperException))]
        public async Task CreateCategoriesViewModelAsync_ThrowsWebUiHelperException()
        {
            _categoryService.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<Category, bool>>>())).Throws<ServiceException>();

            var target = new CategoryHelper(_categoryService.Object);
            await target.CreateCategoriesViewModelAsync(1, 1, null);
        }

        [TestCategory("CategoryHelperTests")]
        [TestMethod]
        public async Task CreateCategoriesViewModelAsync_ThrowsWebUiHelperException_WithInnerServiceException()
        {
            try
            {
                _categoryService.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<Category, bool>>>())).Throws<ServiceException>();

                var target = new CategoryHelper(_categoryService.Object);
                await target.CreateCategoriesViewModelAsync(1, 1, null);
            }
            catch (WebUiHelperException e)
            {
                Assert.IsInstanceOfType(e.InnerException, typeof(ServiceException));
            }

        }

        [TestCategory("CategoryHelperTests")]
        [TestMethod]
        [ExpectedException(typeof(WebUiHelperException))]
        public async Task GetCategoriesToShowOnPageAsync_ThrowsWebUiHelperException()
        {
            _categoryService.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<Category, bool>>>())).Throws<ServiceException>();

            var target = new CategoryHelper(_categoryService.Object);
            await target.GetCategoriesToShowOnPageAsync(1, 1, null);
        }

        [TestCategory("CategoryHelperTests")]
        [TestMethod]
        public async Task GetCategoriesToShowOnPageAsync_ThrowsWebUiHelperException_WithInnerServiceException()
        {
            _categoryService.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<Category, bool>>>())).Throws<ServiceException>();

            try
            {
                var target = new CategoryHelper(_categoryService.Object);
                await target.GetCategoriesToShowOnPageAsync(1, 1, null);
            }
            catch (WebUiHelperException e)
            {
                Assert.IsInstanceOfType(e.InnerException, typeof(ServiceException));
            }
        }
    }
}
