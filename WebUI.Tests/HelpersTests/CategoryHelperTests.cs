using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services;
using Moq;
using HomeAccountingSystem_WebUI.Helpers;
using DomainModels.Model;
using System.Collections.Generic;
using System.Linq;

namespace WebUI.Tests.HelpersTests
{
    [TestClass]
    public class CategoryHelperTests
    {
        private readonly Mock<ICategoryService> _categoryService;
        
        private List<Category> categories => new List<Category>()
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
        public async Task GetCategoriesToShowOnPage_CheckByUserId()
        {

            _categoryService.Setup(m => m.GetListAsync()).ReturnsAsync(categories);
            var target = new CategoryHelper(_categoryService.Object);

            var result = (await target.GetCategoriesToShowOnPage(1, 7, CheckByUserId)).ToList();
            
            Assert.AreEqual(2, result.Count());

            bool CheckByUserId(Category category)
            {
                if (category.UserId == "1")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        [TestCategory("CategoryHelperTests")]
        [TestMethod]
        public async Task GetCategoriesToShowOnPage_CheckByUserIdAndTypeOfFlowId()
        {
            _categoryService.Setup(m => m.GetListAsync()).ReturnsAsync(categories);
            var target = new CategoryHelper(_categoryService.Object);

            var result = (await target.GetCategoriesToShowOnPage(1, 7, CheckByUserIdTypeOfFlowId)).ToList();

            Assert.AreEqual(1, result.Count());

            bool CheckByUserIdTypeOfFlowId(Category category)
            {
                if (category.UserId == "1" && category.TypeOfFlowID == 2)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
