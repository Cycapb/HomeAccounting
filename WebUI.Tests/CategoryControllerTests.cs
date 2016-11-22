using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using HomeAccountingSystem_DAL.Model;
using HomeAccountingSystem_DAL.Repositories;
using HomeAccountingSystem_WebUI.Abstract;
using HomeAccountingSystem_WebUI.Controllers;
using HomeAccountingSystem_WebUI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace WebUI.Tests
{
    [TestClass]
    public class CategoryControllerTests
    {
        [TestMethod]
        public void Can_Create_TypesOfFlow()
        {
            Mock<IRepository<TypeOfFlow>> mock = new Mock<IRepository<TypeOfFlow>>();
            mock.Setup(m => m.GetList()).Returns(new List<TypeOfFlow>()
            {
                new TypeOfFlow() {TypeID = 1,TypeName = "Input"},
                new TypeOfFlow() {TypeID = 2,TypeName = "Output"}
            });
            NavTypeOfFlowController target = new NavTypeOfFlowController(mock.Object);

            var result = target.List().ViewData.Model as List<TypeOfFlow>;

            Assert.IsTrue(result.Count != 0);
            Assert.AreEqual(result[0].TypeID,1);
            Assert.AreEqual(result[1].TypeID,2);
        }

        [TestMethod]
        public void Indicates_Selected_TypeOfFlow()
        {
            Mock<IRepository<TypeOfFlow>> mock = new Mock<IRepository<TypeOfFlow>>();
            mock.Setup(m => m.GetList()).Returns(new List<TypeOfFlow>()
            {
                new TypeOfFlow() {TypeID = 1,TypeName = "Input"},
                new TypeOfFlow() {TypeID = 2,TypeName = "Output"}
            });
            NavTypeOfFlowController target = new NavTypeOfFlowController(mock.Object);

            var result = ((PartialViewResult) target.List()).Model as IEnumerable<TypeOfFlow>;

            Assert.IsNotNull(result);
        }

        //[TestMethod]
        //public async Task Can_Save_Valid_Category_Changes()
        //{
        //    Mock<IRepository<Category>> mock = new Mock<IRepository<Category>>();
        //    Category category = new Category() {CategoryID = 1,Name = "Test"};
        //    CategoryController target = new CategoryController(mock.Object,null,null);

        //    var result = await target.Edit(category);

        //    mock.Verify(m=>m.SaveAsync());
        //    Assert.IsNotInstanceOfType(result,typeof(ViewResult));
        //}

        //[TestMethod]
        //public async Task Cannot_Save_Invalid_Category_Changes()
        //{
        //    Mock<IRepository<Category>> mock = new Mock<IRepository<Category>>();
        //    Mock<IRepository<TypeOfFlow>> mockTof = new Mock<IRepository<TypeOfFlow>>();
        //    mockTof.Setup(m => m.GetList()).Returns(new List<TypeOfFlow>()
        //    {
        //        new TypeOfFlow() {TypeID = 1, TypeName = "Type1"},
        //        new TypeOfFlow() {TypeID = 2, TypeName = "Type2"}
        //    });
        //    Category category = new Category() { CategoryID = 1, Name = "Test" };
        //    CategoryController target = new CategoryController(mock.Object, mockTof.Object,null,null);
        //    target.ModelState.AddModelError("error","error");

        //    var result = await target.Edit(category);

        //    mock.Verify(m=>m.SaveAsync(),Times.Never);
        //    Assert.IsInstanceOfType(result,typeof(PartialViewResult));
        //}

        //[TestMethod]
        //public async Task Can_Add_Valid_Category()
        //{
        //    Mock<IRepository<Category>> mock = new Mock<IRepository<Category>>();
        //    Mock<IPlanningHelper> mockPlaningHelper = new Mock<IPlanningHelper>();
        //    Category category = new Category() {CategoryID = 1,Name = "Cat1"};
        //    CategoryController target = new CategoryController(mock.Object,null,mockPlaningHelper.Object,null);

        //    var result = await target.Add(new WebUser() {Id = "1"}, category);

        //    mock.Verify(m=>m.SaveAsync());
        //    Assert.AreNotEqual(result,typeof(ViewResult));
        //}

        //[TestMethod]
        //public async Task Cannot_Add_Invalid_Category()
        //{
        //    Mock<IRepository<Category>> mock = new Mock<IRepository<Category>>();
        //    Category category = new Category() {CategoryID = 1,Name = "Cat1"};
        //    Mock<IRepository<TypeOfFlow>> mockTof = new Mock<IRepository<TypeOfFlow>>();
        //    mockTof.Setup(m => m.GetList()).Returns(new List<TypeOfFlow>()
        //    {
        //        new TypeOfFlow() {TypeID = 1, TypeName = "Type1"},
        //        new TypeOfFlow() {TypeID = 2, TypeName = "Type2"}
        //    });
        //    CategoryController target = new CategoryController(mock.Object,mockTof.Object,null,null);
        //    target.ModelState.AddModelError("error","error");

        //    var result = await target.Add(new WebUser() {Id = "1"}, category);

        //    mock.Verify(m=>m.SaveAsync(),Times.Never);
        //    Assert.IsInstanceOfType(result,typeof(PartialViewResult));
        //}
    }
}
