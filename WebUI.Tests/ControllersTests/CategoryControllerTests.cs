using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using DomainModels.Model;
using WebUI.Abstract;
using WebUI.Controllers;
using WebUI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Services;
using System;

namespace WebUI.Tests.ControllerTests
{
    [TestClass]
    public class CategoryControllerTests
    {
        private readonly Mock<ITypeOfFlowService> _tofService;
        private readonly Mock<ICategoryService> _catService;
        private readonly Mock<IPlanningHelper> _planningHelper;
        private Mock<ICategoryHelper> _categoryHelper;

        public CategoryControllerTests()
        {
            _catService = new Mock<ICategoryService>();
            _tofService = new Mock<ITypeOfFlowService>();
            _planningHelper = new Mock<IPlanningHelper>();
            _categoryHelper = new Mock<ICategoryHelper>();
        }

        [TestMethod]
        [TestCategory("CategoryControllerTests")]
        public void Can_Create_TypesOfFlow()
        {
            _tofService.Setup(m => m.GetList()).Returns(new List<TypeOfFlow>()
            {
                new TypeOfFlow() {TypeID = 1,TypeName = "Input"},
                new TypeOfFlow() {TypeID = 2,TypeName = "Output"}
            });
            NavTypeOfFlowController target = new NavTypeOfFlowController(_tofService.Object);

            var result = target.List().ViewData.Model as List<TypeOfFlow>;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count != 0);
            Assert.AreEqual(result[0].TypeID,1);
            Assert.AreEqual(result[1].TypeID,2);
        }


        [TestCategory("CategoryControllerTests")]
        [TestMethod]
        public void Indicates_Selected_TypeOfFlow()
        {
            _tofService.Setup(m => m.GetList()).Returns(new List<TypeOfFlow>()
            {
                new TypeOfFlow() {TypeID = 1,TypeName = "Input"},
                new TypeOfFlow() {TypeID = 2,TypeName = "Output"}
            });
            NavTypeOfFlowController target = new NavTypeOfFlowController(_tofService.Object);

            var result = ((PartialViewResult) target.List()).Model as IEnumerable<TypeOfFlow>;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        [TestCategory("CategoryControllerTests")]
        public async Task Edit_InputCategory_ValidModelState_ReturnsRedirectToAction()
        {
            CategoryController target = new CategoryController(_tofService.Object, null, _catService.Object, null);

            var result = await target.Edit(new Category());

            _catService.Verify(m => m.UpdateAsync(It.IsAny<Category>()), Times.Exactly(1));
            _catService.Verify(m => m.SaveAsync(), Times.Exactly(1));
            Assert.IsNotInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        [TestCategory("CategoryControllerTests")]
        public async Task Edit_InputCategory_InvalidModelState_ReturnsPartialViewResult()
        {
            CategoryController target = new CategoryController(_tofService.Object, null, _catService.Object, null);
            target.ModelState.AddModelError("error", "error");

            var result = await target.Edit(new Category());
            var model = (result as PartialViewResult).ViewBag.TypesOfFlow;

            _tofService.Verify(m => m.GetListAsync(), Times.Exactly(1));
            Assert.IsInstanceOfType(model, typeof(IEnumerable<TypeOfFlow>));
            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
        }

        [TestMethod]
        [TestCategory("CategoryControllerTests")]
        public async Task Edit_CategoryNull_ReturnsRedirectToIndex()
        {
            Category category = null;
            var target = new CategoryController(_tofService.Object, null, _catService.Object, null);
            _catService.Setup(m => m.GetItemAsync(It.IsAny<int>())).ReturnsAsync(category);

            var result = await target.Edit(It.IsAny<int>());

            _tofService.Verify(m => m.GetListAsync(), Times.Exactly(1));
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }

        [TestMethod]
        [TestCategory("CategoryControllerTests")]
        public async Task Edit_CategoryNotNull_ReturnsPartialView()
        {
            var target = new CategoryController(_tofService.Object, null, _catService.Object, null);
            _catService.Setup(m => m.GetItemAsync(It.IsAny<int>())).ReturnsAsync(new Category());

            var result = await target.Edit(It.IsAny<int>());

            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
        }

        [TestMethod]
        [TestCategory("CategoryControllerTests")]
        public async Task Add_InputWebUser_ReturnsPartialView()
        {
            var target = new CategoryController(_tofService.Object,null, null, null);

            var result = await target.Add(new WebUser() { Id = "1" });
            var model = (result as PartialViewResult).ViewBag.TypesOfFlow;

            _tofService.Verify(m => m.GetListAsync(), Times.Exactly(1));
            Assert.IsInstanceOfType(model, typeof(IEnumerable<TypeOfFlow>));
            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
        }

        [TestMethod]
        [TestCategory("CategoryControllerTests")]
        public async Task Can_Add_Valid_Category_Rerurns_RedirectToAction()
        {
            var target = new CategoryController(null, _planningHelper.Object, _catService.Object, null);

            var result = await target.Add(new WebUser() { Id = "1" }, new Category() { CategoryID = 1, Name = "Cat1" });

            _catService.Verify(m => m.CreateAsync(It.IsAny<Category>()), Times.Exactly(1));
            _planningHelper.Verify(m => m.CreatePlanItemsForCategory(It.IsAny<WebUser>(), It.IsAny<int>()),Times.Exactly(1));
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));            
        }

        [TestMethod]
        [TestCategory("CategoryControllerTests")]
        public async Task Cannot_Add_Invalid_Category()
        {
            Category category = new Category() { CategoryID = 1, Name = "Cat1" };
            _tofService.Setup(m => m.GetList()).Returns(new List<TypeOfFlow>()
            {
                new TypeOfFlow() {TypeID = 1, TypeName = "Type1"},
                new TypeOfFlow() {TypeID = 2, TypeName = "Type2"}
            });
            var target = new CategoryController(_tofService.Object, _planningHelper.Object, null, null);
            target.ModelState.AddModelError("error", "error");

            var result = await target.Add(new WebUser() { Id = "1" }, category);

            _catService.Verify(m => m.SaveAsync(), Times.Never);
            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
        }

        [TestMethod]
        [TestCategory("CategoryControllerTests")]
        public async Task Delete_CannotDeleteIfHasAnyDependencies()
        {
            var target = new CategoryController(null, null, _catService.Object, null);
            _catService.Setup(m => m.HasDependencies(It.IsAny<int>())).ReturnsAsync(true);

            var result = await target.Delete(It.IsAny<int>());

            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }

        [TestMethod]
        [TestCategory("CategoryControllerTests")]
        public async Task Delete_CanDeleteIfHasNoDependencies()
        {
            var target = new CategoryController(null, null, _catService.Object, null);
            _catService.Setup(m => m.HasDependencies(It.IsAny<int>())).ReturnsAsync(false);

            var result = await target.Delete(It.IsAny<int>());

            _catService.Verify(m => m.DeleteAsync(It.IsAny<int>()), Times.Exactly(1));
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }

        [TestMethod]
        [TestCategory("CategoryControllerTests")]
        public async Task GetCategoriesAndPages_ReturnsPartialView()
        {
            var target = new CategoryController(null, null, null, _categoryHelper.Object);
            _categoryHelper.Setup(m => m.CreateCategoriesViewModel(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Func<Category, bool>>())).ReturnsAsync(new CategoriesViewModel());

            var result = await target.GetCategoriesAndPages(new WebUser(), 1);

            Assert.IsInstanceOfType(result, typeof(PartialViewResult));            
        }

        [TestMethod]
        [TestCategory("CategoryControllerTests")]
        public async Task GetCategoriesAndPagesByType_ReturnsPartialView()
        {
            var target = new CategoryController(null, null, null, _categoryHelper.Object);
            _categoryHelper.Setup(m => m.CreateCategoriesViewModel(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Func<Category, bool>>())).ReturnsAsync(new CategoriesViewModel());

            var result = await target.GetCategoriesAndPagesByType(new WebUser(), 1, 1);
            var model = (result as PartialViewResult).Model as CategoriesViewModel;

            Assert.AreEqual(model.TypeOfFlowId, 1);
            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
        }

        [TestMethod]
        [TestCategory("CategoryControllerTests")]
        public async Task GetCategoriesByType_InputTypeOfFlowIdPage_ReturnsPartialView()
        {
            var target = new CategoryController(null, null, null, _categoryHelper.Object);
            _categoryHelper.Setup(m => m.GetCategoriesToShowOnPage(It.IsAny<int>(), It.IsAny<int>(),It.IsAny<Func<Category, bool>>())).ReturnsAsync(new List<Category>());

            var result = await target.GetCategoriesByType(new WebUser(), 1, 1);
            var viewName = (result as PartialViewResult).ViewName;

            Assert.AreEqual(viewName, "CategorySummaryPartial");
            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
        }

        [TestMethod]
        [TestCategory("CategoryControllerTests")]
        public async Task GetAllCategories_InputPage_returnsPartialView()
        {
            var target = new CategoryController(null, null, null, _categoryHelper.Object);
            _categoryHelper.Setup(m => m.GetCategoriesToShowOnPage(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Func<Category, bool>>())).ReturnsAsync(new List<Category>());

            var result = await target.GetCategoriesByType(new WebUser(), 1, 1);
            var viewName = (result as PartialViewResult).ViewName;

            Assert.AreEqual(viewName, "CategorySummaryPartial");
            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
        }

        [TestMethod]
        [TestCategory("CategoryControllerTests")]
        public async Task Index_ReturnsPartialView()
        {
            var target = new CategoryController(null, null, null, _categoryHelper.Object);
            _categoryHelper.Setup(m => m.CreateCategoriesViewModel(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Func<Category, bool>>())).ReturnsAsync(new CategoriesViewModel());

            var result = await target.Index(new WebUser(), 1);
            var model = (result as PartialViewResult).Model as CategoriesViewModel;

            Assert.IsNotNull(model);
            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
        }

        [TestMethod]
        [TestCategory("CategoryControllerTests")]
        public async Task GetAllCategories_ReturnsPartialViewresult()
        {
            var target = new CategoryController(null, null, null, _categoryHelper.Object);
            _categoryHelper.Setup(m => m.GetCategoriesToShowOnPage(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Func<Category, bool>>())).ReturnsAsync(new List<Category>());

            var result = await target.GetAllCategories(new WebUser(), 1);
            var viewName = (result as PartialViewResult).ViewName;
            var model = (result as PartialViewResult).Model as IEnumerable<Category>;

            Assert.IsNotNull(model);
            Assert.AreEqual(viewName, "CategorySummaryPartial");
            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
        }
    }
}
