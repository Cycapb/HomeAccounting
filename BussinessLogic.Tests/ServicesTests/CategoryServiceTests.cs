﻿using System.Collections.Generic;
using System.Linq;
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

            var result = await _service.GetActiveGategoriesByUser(It.IsAny<string>());            

            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        [TestCategory("CategoryServiceTests")]
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
