using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using HomeAccountingSystem_DAL.Model;
using HomeAccountingSystem_WebUI.Controllers;
using HomeAccountingSystem_WebUI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Services;

namespace HomeAccountingSystem_UnitTests
{
    [TestClass]
    public class ProductTests
    {
        private readonly Mock<IProductService> _productServiceMock;
        private readonly Mock<ICategoryService> _categoryServiceMock;

        public ProductTests()
        {
            _productServiceMock = new Mock<IProductService>();
            _categoryServiceMock = new Mock<ICategoryService>();
        }

        [TestMethod]
        public async Task Can_Add_Valid_Product()
        {
            Product product = new Product() {CategoryID = 1,Description = "Prod1",ProductID = 1};
            var target = new ProductController(_productServiceMock.Object);

            var result = await target.Add(new WebUser() {Id = "1"}, product);

            _productServiceMock.Verify(m=>m.CreateAsync(product));
            Assert.IsInstanceOfType(result,typeof(RedirectToRouteResult));
        }

        [TestMethod]
        public async Task Cannot_Add_Invalid_Product()
        {
            Product product = new Product() { CategoryID = 1, Description = "Prod1", ProductID = 1 };
            var target = new ProductController(_productServiceMock.Object);

            target.ModelState.AddModelError("error", "error");
            var result = await target.Add(new WebUser() { Id = "1" }, product);

            _productServiceMock.Verify(m=>m.CreateAsync(product),Times.Never);
            Assert.IsNotInstanceOfType(result,typeof(RedirectToRouteResult));
        }

        [TestMethod]
        public void Can_List_Products()
        {
            _productServiceMock.Setup(m => m.GetList()).Returns(new List<Product>
            {
                new Product() {CategoryID = 1,ProductID = 1,UserID = "1"},
                new Product() {CategoryID = 1,ProductID = 2,UserID = "1"},
                new Product() {CategoryID = 2,ProductID = 3,UserID = "1"},
                new Product() {CategoryID = 3,ProductID = 2,UserID = "1"},
            });
            var target = new ProductController(_productServiceMock.Object);
            var categoryId = 1;

            var result = target.List(categoryId).ViewData.Model as List<Product>;

            Assert.AreEqual(result.Count,2);
            Assert.AreEqual(result[0].CategoryID,categoryId);
            Assert.AreEqual(result[1].CategoryID,categoryId);
        }


        [TestMethod]
        public async Task Can_Create_Valid_ProductModel_For_Edit()
        {
            _productServiceMock.Setup(m => m.GetList()).Returns(new List<Product>
            {
                new Product() {CategoryID = 1,ProductID = 1,UserID = "1"},
                new Product() {CategoryID = 1,ProductID = 2,UserID = "1"},
                new Product() {CategoryID = 2,ProductID = 3,UserID = "1"},
                new Product() {CategoryID = 3,ProductID = 4,UserID = "1"},
            });
            _categoryServiceMock.Setup(m => m.GetListAsync()).ReturnsAsync(new List<Category>
            {
                new Category() {CategoryID = 1,UserId = "1"},
                new Category() {CategoryID = 2,UserId = "1"},
                new Category() {CategoryID = 3,UserId = "1"}
            });
            var target = new ProductController(_productServiceMock.Object);
            var productId = 1;
            _productServiceMock.Setup(m => m.GetItemAsync(productId)).ReturnsAsync(new Product() { CategoryID = 1, ProductID = 1, UserID = "1" });

            var result = await target.Edit(new WebUser(),productId);
            var productToEdit = target.ViewData.Model as ProductToEdit;

            Assert.AreEqual(productToEdit.Product.ProductID,productId);
            Assert.IsInstanceOfType(result,typeof(PartialViewResult));
        }

        [TestMethod]
        public async Task Can_Edit_Valid_Product()
        {
            ProductToEdit productToEdit = new ProductToEdit()
            {
                Product = new Product() {CategoryID = 1,ProductID = 1}
            };
            var target = new ProductController(_productServiceMock.Object);

            var result = await target.Edit(productToEdit);

            _productServiceMock.Verify(m => m.UpdateAsync(productToEdit.Product));
            Assert.IsInstanceOfType(result,typeof(RedirectToRouteResult));
        }

        [TestMethod]
        public async Task Cannot_Edit_Invalid_Product()
        {
            var target = new ProductController(_productServiceMock.Object);
            ProductToEdit pToEdit = new ProductToEdit();
            Product product = new Product();

            target.ModelState.AddModelError("error","error");
            var result = await target.Edit(pToEdit);

            Assert.IsNotInstanceOfType(result,typeof(RedirectToRouteResult));
        }
    }
}
