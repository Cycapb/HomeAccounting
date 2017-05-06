using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using DomainModels.Model;
using HomeAccountingSystem_WebUI.Abstract;
using HomeAccountingSystem_WebUI.Controllers;
using HomeAccountingSystem_WebUI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Services;

namespace WebUI.Tests
{
    [TestClass]
    public class CategoryControllerTests
    {
        private readonly Mock<ITypeOfFlowService> _tofService;
        private readonly Mock<ICategoryService> _catService;
        private readonly Mock<IPlanningHelper> _planningHelper;

        public CategoryControllerTests()
        {
            _catService = new Mock<ICategoryService>();
            _tofService = new Mock<ITypeOfFlowService>();
            _planningHelper = new Mock<IPlanningHelper>();
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
        public async Task Can_Save_Valid_Category_Changes()
        {
            Category category = new Category() { CategoryID = 1, Name = "Test" };
            CategoryController target = new CategoryController(_tofService.Object, null, _catService.Object);

            var result = await target.Edit(category);

            _catService.Verify(m => m.UpdateAsync(It.IsAny<Category>()), Times.Exactly(1));
            _catService.Verify(m => m.SaveAsync(), Times.Exactly(1));
            Assert.IsNotInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        [TestCategory("CategoryControllerTests")]
        public async Task Cannot_Save_Invalid_Category_Changes()
        {
            _tofService.Setup(m => m.GetList()).Returns(new List<TypeOfFlow>()
            {
                new TypeOfFlow() {TypeID = 1, TypeName = "Type1"},
                new TypeOfFlow() {TypeID = 2, TypeName = "Type2"}
            });
            Category category = new Category() { CategoryID = 1, Name = "Test" };
            CategoryController target = new CategoryController(_tofService.Object, null, _catService.Object);
            target.ModelState.AddModelError("error", "error");

            var result = await target.Edit(category);

            _catService.Verify(m => m.SaveAsync(), Times.Never);
            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
        }

        [TestMethod]
        [TestCategory("CategoryControllerTests")]
        public async Task Edit_CategoryNull_ReturnsRedirectToIndex()
        {
            var target = new CategoryController(_tofService.Object, null, _catService.Object);
            _catService.Setup(m => m.GetItemAsync(It.IsAny<int>())).ReturnsAsync(null);

            var result = await target.Edit(It.IsAny<int>());

            _tofService.Verify(m => m.GetListAsync(), Times.Exactly(1));
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }

        [TestMethod]
        [TestCategory("CategoryControllerTests")]
        public async Task Edit_CategoryNotNull_ReturnsPartialView()
        {
            var target = new CategoryController(_tofService.Object, null, _catService.Object);
            _catService.Setup(m => m.GetItemAsync(It.IsAny<int>())).ReturnsAsync(new Category());

            var result = await target.Edit(It.IsAny<int>());

            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
        }

        [TestMethod]
        [TestCategory("CategoryControllerTests")]
        public async Task Add_InputWebUser_ReturnsPartialView()
        {
            var target = new CategoryController(_tofService.Object,null, null);

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
            var target = new CategoryController(null, _planningHelper.Object, _catService.Object);

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
            var target = new CategoryController(_tofService.Object, _planningHelper.Object, null);
            target.ModelState.AddModelError("error", "error");

            var result = await target.Add(new WebUser() { Id = "1" }, category);

            _catService.Verify(m => m.SaveAsync(), Times.Never);
            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
        }

        [TestMethod]
        [TestCategory("CategoryControllerTests")]
        public async Task Delete_CannotDeleteIfHasAnyDependencies()
        {
            var target = new CategoryController(null, null, _catService.Object);
            _catService.Setup(m => m.HasDependencies(It.IsAny<int>())).ReturnsAsync(true);

            var result = await target.Delete(It.IsAny<int>());

            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }

        [TestMethod]
        [TestCategory("CategoryControllerTests")]
        public async Task Delete_CanDeleteIfHasNoDependencies()
        {
            var target = new CategoryController(null, null, _catService.Object);
            _catService.Setup(m => m.HasDependencies(It.IsAny<int>())).ReturnsAsync(false);

            var result = await target.Delete(It.IsAny<int>());

            _catService.Verify(m => m.DeleteAsync(It.IsAny<int>()), Times.Exactly(1));
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }

        [TestMethod]
        [TestCategory("CategoryControllerTests")]
        public async Task GetCategoriesAndPages_ReturnsPartialView()
        {

        }
    }
}
