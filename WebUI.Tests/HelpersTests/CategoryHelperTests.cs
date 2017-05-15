using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services;
using Moq;
using HomeAccountingSystem_WebUI.Helpers;
using DomainModels.Model;
using System.Collections.Generic;

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
            new Category(){ Name = "C5", UserId = "1"},
        };

        public CategoryHelperTests()
        {
            _categoryService = new Mock<ICategoryService>();            
        }
        
        [TestCategory("CategoryHelperTests")]
        [TestMethod]
        public async Task GetCategoriesToShowOnPage()
        {

            _categoryService.Setup(m => m.GetListAsync()).ReturnsAsync(categories);
            var target = new CategoryHelper(_categoryService.Object);

            var result = (await target.GetCategoriesToShowOnPage(1, 1, CheckByUserId));
            
            //ToDo Write assertions

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
    }
}
