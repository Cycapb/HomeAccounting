using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BussinessLogic.Services;
using HomeAccountingSystem_DAL.Model;
using HomeAccountingSystem_DAL.Repositories;
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
        public async Task GetActiveGategoriesByUser_ReturnsNull()
        {
            _catRepository.Setup(m => m.GetListAsync()).ReturnsAsync(null);

            var result = await _service.GetActiveGategoriesByUser(It.IsAny<string>());

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetActiveGategoriesByUser_ReturnsListOfCategories()
        {
            _catRepository.Setup(m => m.GetListAsync()).ReturnsAsync(new List<Category>()
            {
                new Category() {CategoryID = 1,Name = "C1",Active = true,UserId = "1"},
                new Category() {CategoryID = 2, Name = "C2",Active = true,UserId = "1"},
                new Category() {CategoryID = 3, Name = "C3",Active = true,UserId = "2"}
            });

            var result = (await _service.GetActiveGategoriesByUser("1")).ToList();

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count,2);
        }
    }
}
