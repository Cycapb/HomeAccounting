using DomainModels.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Services;
using Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebUI.Core.Abstract;
using WebUI.Core.Controllers;
using WebUI.Core.Exceptions;
using WebUI.Core.Models;
using WebUI.Core.Models.CategoryModels;
using System.Linq.Expressions;

namespace WebUI.Core.Tests.ControllersTests
{
    [TestClass]
    public class CategoryControllerTests
    {
        private readonly Mock<ITypeOfFlowService> _typeOfFlowService;
        private readonly Mock<ICategoryService> _categoryService;
        private readonly Mock<ICategoryHelper> _categoryHelper;

        public CategoryControllerTests()
        {
            _categoryService = new Mock<ICategoryService>();
            _typeOfFlowService = new Mock<ITypeOfFlowService>();
            _categoryHelper = new Mock<ICategoryHelper>();
        }

        [TestMethod]
        [TestCategory("CategoryControllerTests")]
        public async Task Edit_HttpPost_InputCategory_ValidModelState_ReturnsRedirectToActionGetCategoriesAndPages()
        {
            var target = new CategoryController(_typeOfFlowService.Object, _categoryService.Object, null);

            var result = await target.Edit(new Category());
            var actionName = ((RedirectToActionResult)result).ActionName;
            
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            Assert.AreEqual(nameof(CategoryController.GetCategoriesAndPages), actionName);
        }

        [TestMethod]
        [TestCategory("CategoryControllerTests")]
        public async Task Edit_HttpPost_InputCategory_InvalidModelState_ReturnsPartialViewResult()
        {
            CategoryController target = new CategoryController(_typeOfFlowService.Object, _categoryService.Object, null);
            target.ModelState.AddModelError("error", "error");

            var result = await target.Edit(new Category());
            var typesOfFlowFromViewData = (result as PartialViewResult).ViewData["TypesOfFlow"];           
            
            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
            Assert.IsInstanceOfType(typesOfFlowFromViewData, typeof(IEnumerable<TypeOfFlow>));
        }

        [TestMethod]
        [TestCategory("CategoryControllerTests")]
        public async Task Edit_HttpGet_CategoryIsNull_ReturnsRedirectToIndex()
        {
            Category category = null;
            var target = new CategoryController(_typeOfFlowService.Object, _categoryService.Object, null);
            _categoryService.Setup(m => m.GetItemAsync(It.IsAny<int>())).ReturnsAsync(category);

            var result = await target.Edit(It.IsAny<int>());
            
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
        }

        [TestMethod]
        [TestCategory("CategoryControllerTests")]
        public async Task Edit_HttpGet_CategoryNotNull_ReturnsPartialView()
        {
            var target = new CategoryController(_typeOfFlowService.Object, _categoryService.Object, null);
            _categoryService.Setup(m => m.GetItemAsync(It.IsAny<int>())).ReturnsAsync(new Category());

            var result = await target.Edit(It.IsAny<int>());

            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
        }

        [TestMethod]
        [TestCategory("CategoryControllerTests")]
        public async Task Add_HttpGet_InputWebUser_ReturnsPartialView()
        {
            var target = new CategoryController(_typeOfFlowService.Object, null, null);

            var result = await target.Add(new WebUser() { Id = "1" });
            var typesOfFlowFromViewData = (result as PartialViewResult).ViewData["TypesOfFlow"];

            Assert.IsInstanceOfType(typesOfFlowFromViewData, typeof(IEnumerable<TypeOfFlow>));
            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
        }

        [TestMethod]
        [TestCategory("CategoryControllerTests")]
        public async Task Add_HttpPost_ValidModelState_Category_ReturnsRedirectToAction()
        {
            var target = new CategoryController(null, _categoryService.Object, null);

            var result = await target.Add(new WebUser() { Id = "1" }, new Category() { CategoryID = 1, Name = "Cat1" });
            var actionName = ((RedirectToActionResult)result).ActionName;

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            Assert.AreEqual(nameof(CategoryController.GetCategoriesAndPages), actionName);
        }

        [TestMethod]
        [TestCategory("CategoryControllerTests")]
        public async Task Add_HttpPost_InvalidModelState_ReturnsPartialViewResult()
        {
            var category = new Category() { CategoryID = 1, Name = "Cat1" };
            _typeOfFlowService.Setup(m => m.GetList()).Returns(new List<TypeOfFlow>()
            {
                new TypeOfFlow() {TypeID = 1, TypeName = "Type1"},
                new TypeOfFlow() {TypeID = 2, TypeName = "Type2"}
            });
            var target = new CategoryController(_typeOfFlowService.Object, null, null);
            target.ModelState.AddModelError("error", "error");

            var result = await target.Add(new WebUser() { Id = "1" }, category);

            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
        }

        [TestMethod]
        [TestCategory("CategoryControllerTests")]
        public async Task Delete_CannotDeleteIfHasAnyDependencies_ReturnsRedirectToActionResult()
        {
            var target = new CategoryController(null, _categoryService.Object, null);
            _categoryService.Setup(m => m.HasDependenciesAsync(It.IsAny<int>())).ReturnsAsync(true);

            var result = await target.Delete(It.IsAny<int>());

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
        }

        [TestMethod]
        [TestCategory("CategoryControllerTests")]
        public async Task Delete_CanDeleteIfHasNoDependencies_ReturnsRedirectToActionResult()
        {
            var target = new CategoryController(null, _categoryService.Object, null);
            _categoryService.Setup(m => m.HasDependenciesAsync(It.IsAny<int>())).ReturnsAsync(false);

            var result = await target.Delete(It.IsAny<int>());

            _categoryService.Verify(m => m.DeleteAsync(It.IsAny<int>()), Times.Exactly(1));
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
        }

        [TestMethod]
        [TestCategory("CategoryControllerTests")]
        public async Task GetCategoriesAndPages_ReturnsPartialView()
        {
            var target = new CategoryController(null, null, _categoryHelper.Object);
            _categoryHelper.Setup(m => m.CreateCategoriesViewModelAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Expression<Func<Category, bool>>>())).ReturnsAsync(new CategoriesCollectionModel());

            var result = await target.GetCategoriesAndPages(new WebUser(), 1);

            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
        }

        [TestMethod]
        [TestCategory("CategoryControllerTests")]
        public async Task GetCategoriesAndPagesByType_ReturnsPartialView()
        {
            var target = new CategoryController(null, null, _categoryHelper.Object);
            _categoryHelper.Setup(m => m.CreateCategoriesViewModelAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Expression<Func<Category, bool>>>())).ReturnsAsync(new CategoriesCollectionModel());

            var result = await target.GetCategoriesAndPagesByType(new WebUser(), 1, 1);
            var model = (result as PartialViewResult).Model as CategoriesCollectionModel;

            Assert.AreEqual(model.TypeOfFlowId, 1);
            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
        }

        [TestMethod]
        [TestCategory("CategoryControllerTests")]
        public async Task GetCategoriesAndPagesByType_InputTypeOfFlowIdPage_ReturnsPartialView()
        {
            var target = new CategoryController(null, null, _categoryHelper.Object);
            _categoryHelper.Setup(m => m.CreateCategoriesViewModelAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Expression<Func<Category, bool>>>())).ReturnsAsync(new CategoriesCollectionModel());

            var result = await target.GetCategoriesAndPagesByType(new WebUser(), 1, 1);
            var viewName = (result as PartialViewResult).ViewName;

            Assert.AreEqual(viewName, "_CategoriesAndPagesByType");
            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
        }

        [TestMethod]
        [TestCategory("CategoryControllerTests")]
        public async Task GetCategoriesAndPagesByType_InputPage_ReturnsPartialView()
        {
            var target = new CategoryController(null, null, _categoryHelper.Object);
            _categoryHelper.Setup(m => m.CreateCategoriesViewModelAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Expression<Func<Category, bool>>>())).ReturnsAsync(new CategoriesCollectionModel());

            var result = await target.GetCategoriesAndPagesByType(new WebUser(), 1, 1);
            var viewName = (result as PartialViewResult).ViewName;

            Assert.AreEqual(viewName, "_CategoriesAndPagesByType");
            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
        }

        [TestMethod]
        [TestCategory("CategoryControllerTests")]
        public async Task Index_ReturnsPartialViewWithCategories()
        {
            var target = new CategoryController(null, null, _categoryHelper.Object);
            _categoryHelper.Setup(m => m.CreateCategoriesViewModelAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Expression<Func<Category, bool>>>())).ReturnsAsync(new CategoriesCollectionModel());

            var result = await target.Index(new WebUser(), 1);
            var model = (result as PartialViewResult).Model as CategoriesCollectionModel;

            Assert.IsNotNull(model);
            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
        }

        [TestMethod]
        [TestCategory("CategoryControllerTests")]
        public async Task GetAllCategories_ReturnsPartialViewResultWithCategoriesForUser()
        {
            var target = new CategoryController(null, null, _categoryHelper.Object);
            _categoryHelper.Setup(m => m.GetCategoriesToShowOnPageAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Expression<Func<Category, bool>>>())).ReturnsAsync(new List<Category>());

            var result = await target.GetAllCategories(new WebUser(), 1);
            var viewName = (result as PartialViewResult).ViewName;
            var model = (result as PartialViewResult).Model as IEnumerable<Category>;

            Assert.IsNotNull(model);
            Assert.AreEqual(viewName, "_CategorySummary");
            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
        }

        [TestCategory("CategoryControllerTests")]
        [TestMethod]
        [ExpectedException(typeof(WebUiException))]
        public async Task Edit_ThrowsWebuiException()
        {
            _categoryService.Setup(x => x.GetItemAsync(It.IsAny<int>())).Throws<ServiceException>();
            _typeOfFlowService.Setup(x => x.GetListAsync()).ReturnsAsync(new List<TypeOfFlow>());
            var target = new CategoryController(_typeOfFlowService.Object, _categoryService.Object, null);

            await target.Edit(1);
        }

        [TestCategory("CategoryControllerTests")]
        [TestMethod]
        public async Task Edit_ThrowsWebuiException_WithInnerServiceException()
        {
            _categoryService.Setup(x => x.GetItemAsync(It.IsAny<int>())).Throws<ServiceException>();
            _typeOfFlowService.Setup(x => x.GetListAsync()).ReturnsAsync(new List<TypeOfFlow>());
            var target = new CategoryController(_typeOfFlowService.Object, _categoryService.Object, null);

            try
            {
                await target.Edit(1);
            }
            catch (WebUiException e)
            {
                Assert.IsInstanceOfType(e.InnerException, typeof(ServiceException));
            }
        }

        [TestMethod]
        [TestCategory("CategoryControllerTests")]
        [ExpectedException(typeof(WebUiException))]
        public async Task Index_RaisesWebUiHelperException()
        {
            _categoryHelper
                .Setup(m => m.CreateCategoriesViewModelAsync(It.IsAny<int>(), It.IsAny<int>(),
                    It.IsAny<Expression<Func<Category, bool>>>())).Throws<WebUiHelperException>();
            var target = new CategoryController(null, null, _categoryHelper.Object);

            await target.Index(new WebUser());
        }
    }
}
