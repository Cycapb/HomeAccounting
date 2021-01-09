using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BussinessLogic.Services;
using DomainModels.Model;
using DomainModels.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BussinessLogic.Tests.ServicesTests
{
    [TestClass]
    public class CategoryServiceTests
    {
        private readonly Mock<IRepository<Category>> _catRepository;
        private readonly CategoryService _service;

        public CategoryServiceTests()
        {
            _catRepository = new Mock<IRepository<Category>>();
            _service = new CategoryService(_catRepository.Object);
        }

        [TestMethod]
        [TestCategory("CategoryServiceTests")]
        public async Task GetActiveGategoriesByUser_ReturnsZero()
        {            
            _catRepository.Setup(m => m.GetListAsync()).ReturnsAsync(new List<Category>());            

            var result = await _service.GetActiveGategoriesByUserAsync(It.IsAny<string>());            

            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        [TestCategory("CategoryServiceTests")]
        public async Task GetActiveGategoriesByUser_ReturnsListOfCategories()
        {
            var userId = "1";
            var category = new Category() { CategoryID = 3, Name = "C1", Active = true, UserId = "1" };
            var categories = new List<Category>()
            {
                new Category() {CategoryID = 1,Name = "C1",Active = true,UserId = "1"},
                new Category() {CategoryID = 2, Name = "C2",Active = true,UserId = "1"},
                new Category() {CategoryID = 3, Name = "C3",Active = true,UserId = "2"}
            };
            _catRepository.Setup(m => m.GetListAsync(It.Is<Expression<Func<Category, bool>>>(x => x.Compile()(category))))
                .ReturnsAsync(categories.Where(x => x.Active && x.UserId == userId));

            var result = (await _service.GetActiveGategoriesByUserAsync(userId)).ToList();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
        }
    }
}
