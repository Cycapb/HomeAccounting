﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using DomainModels.Model;
using WebUI.Controllers;
using WebUI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Services;
using Services.Exceptions;
using WebUI.Exceptions;
using System.Linq.Expressions;
using System.Linq;

namespace WebUI.Tests.ControllersTests
{
    [TestClass]
    public class ProductControllerTests
    {
        private readonly Mock<IProductService> _productServiceMock;
        private readonly Mock<ICategoryService> _categoryServiceMock;

        public ProductControllerTests()
        {
            _productServiceMock = new Mock<IProductService>();
            _categoryServiceMock = new Mock<ICategoryService>();
        }

        [TestMethod]
        [TestCategory("ProductControllerTests")]
        public async Task Can_Add_Valid_Product()
        {
            Product product = new Product() {CategoryID = 1,Description = "Prod1",ProductID = 1};
            var target = new ProductController(_productServiceMock.Object);

            var result = await target.Add(new WebUser() {Id = "1"}, product);

            _productServiceMock.Verify(m=>m.CreateAsync(product));
            Assert.IsInstanceOfType(result,typeof(RedirectToRouteResult));
        }

        [TestMethod]
        [TestCategory("ProductControllerTests")]
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
        [TestCategory("ProductControllerTests")]
        [ExpectedException(typeof(WebUiException))]
        public async Task Add_RaisesWebUiException()
        {
            _productServiceMock.Setup(m => m.CreateAsync(It.IsAny<Product>())).Throws<ServiceException>();
            var target = new ProductController(_productServiceMock.Object);

            await target.Add(new WebUser(), new Product());
        }

        [TestMethod]
        [TestCategory("ProductControllerTests")]
        public async Task Add_RaisesWebUiExceptionWithInnerServiceException()
        {
            _productServiceMock.Setup(m => m.CreateAsync(It.IsAny<Product>())).Throws<ServiceException>();
            var target = new ProductController(_productServiceMock.Object);

            try
            {
                await target.Add(new WebUser(), new Product());
            }
            catch (WebUiException e)
            {
                Assert.IsInstanceOfType(e.InnerException, typeof(ServiceException));
            }
        }

        [TestMethod]
        [TestCategory("ProductControllerTests")]
        public void Can_List_Products()
        {
            var products = new List<Product>
            {
                new Product() {CategoryID = 1,ProductID = 1,UserID = "1"},
                new Product() {CategoryID = 1,ProductID = 2,UserID = "1"},
                new Product() {CategoryID = 2,ProductID = 3,UserID = "1"},
                new Product() {CategoryID = 3,ProductID = 2,UserID = "1"},
            };
            var categoryId = 1;
            _productServiceMock.Setup(m => m.GetList(It.IsAny<Expression<Func<Product, bool>>>())).Returns(products.Where(p => p.CategoryID == categoryId));
            var target = new ProductController(_productServiceMock.Object);            

            var result = target.List(categoryId).ViewData.Model as List<Product>;

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(categoryId, result[0].CategoryID);
            Assert.AreEqual(categoryId, result[1].CategoryID);
        }

        [TestMethod]
        [TestCategory("ProductControllerTests")]
        [ExpectedException(typeof(WebUiException))]
        public void List_RaisesWebUiException()
        {
            _productServiceMock.Setup(m => m.GetList(It.IsAny<Expression<Func<Product, bool>>>())).Throws<ServiceException>();
            var target = new ProductController(_productServiceMock.Object);

            target.List(It.IsAny<int>());
        }

        [TestMethod]
        [TestCategory("ProductControllerTests")]
        public void List_RaisesWebUiExceptionWithInnerServiceException()
        {
            _productServiceMock.Setup(m => m.GetList(It.IsAny<Expression<Func<Product, bool>>>())).Throws<ServiceException>();
            var target = new ProductController(_productServiceMock.Object);

            try
            {
                target.List(It.IsAny<int>());
            }
            catch (WebUiException e)
            {
                Assert.IsInstanceOfType(e.InnerException, typeof(ServiceException));
            }
            
        }

        [TestMethod]
        [TestCategory("ProductControllerTests")]
        [ExpectedException(typeof(WebUiException))]
        public void EditableList_RaisesWebUiException()
        {
            _productServiceMock.Setup(m => m.GetList(It.IsAny<Expression<Func<Product, bool>>>())).Throws<ServiceException>();
            var target = new ProductController(_productServiceMock.Object);

            target.EditableList(It.IsAny<int>());
        }

        [TestMethod]
        [TestCategory("ProductControllerTests")]
        public void EditableList_RaisesWebUiExceptionWithInnerServiceException()
        {
            _productServiceMock.Setup(m => m.GetList()).Throws<ServiceException>();
            var target = new ProductController(_productServiceMock.Object);

            try
            {
                target.EditableList(It.IsAny<int>());
            }
            catch (WebUiException e)
            {
                Assert.IsInstanceOfType(e.InnerException, typeof(ServiceException));
            }
        }

        [TestMethod]
        [TestCategory("ProductControllerTests")]
        [ExpectedException(typeof(WebUiException))]
        public async Task Delete_RaisesWebUiException()
        {
            _productServiceMock.Setup(m => m.GetList()).Throws<ServiceException>();
            var target = new ProductController(_productServiceMock.Object);

            await target.Delete(It.IsAny<int>());
        }

        [TestMethod]
        [TestCategory("ProductControllerTests")]
        public async Task Delete_RaisesWebUiExceptionWithInnerNullReferenceException()
        {
            var target = new ProductController(_productServiceMock.Object);

            try
            {
                await target.Delete(It.IsAny<int>());
            }
            catch (WebUiException e)
            {
                Assert.IsInstanceOfType(e.InnerException, typeof(NullReferenceException));
            }
        }

        [TestMethod]
        [TestCategory("ProductControllerTests")]
        public async Task Delete_RaisesWebUiExceptionWithInnerServiceException()
        {
            _productServiceMock.Setup(m => m.GetItemAsync(It.IsAny<int>())).Throws<ServiceException>();
            _productServiceMock.Setup(m => m.DeleteAsync(It.IsAny<int>())).Throws<ServiceException>();
            var target = new ProductController(_productServiceMock.Object);

            try
            {
                await target.Delete(It.IsAny<int>());
            }
            catch (WebUiException e)
            {
                Assert.IsInstanceOfType(e.InnerException, typeof(ServiceException));
            }
        }

        [TestMethod]
        [TestCategory("ProductControllerTests")]
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
            var productToEdit = target.ViewData.Model as ProductEditModel;

            Assert.IsNotNull(productToEdit);
            Assert.AreEqual(productToEdit.Product.ProductID,productId);
            Assert.IsInstanceOfType(result,typeof(PartialViewResult));
        }

        [TestMethod]
        [TestCategory("ProductControllerTests")]
        public async Task Can_Edit_Valid_Product()
        {
            ProductEditModel productToEdit = new ProductEditModel()
            {
                Product = new Product() {CategoryID = 1,ProductID = 1}
            };
            var target = new ProductController(_productServiceMock.Object);

            var result = await target.Edit(productToEdit);

            _productServiceMock.Verify(m => m.UpdateAsync(productToEdit.Product));
            Assert.IsInstanceOfType(result,typeof(RedirectToRouteResult));
        }

        [TestMethod]
        [TestCategory("ProductControllerTests")]
        public async Task Cannot_Edit_Invalid_Product()
        {
            var target = new ProductController(_productServiceMock.Object);
            ProductEditModel pToEdit = new ProductEditModel();
            Product product = new Product();

            target.ModelState.AddModelError("error","error");
            var result = await target.Edit(pToEdit);

            Assert.IsNotInstanceOfType(result,typeof(RedirectToRouteResult));
        }
    }
}
