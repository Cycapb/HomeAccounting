using System;
using System.Linq;
using DomainModels.Model;
using HomeAccountingSystem_WebUI.Abstract;
using HomeAccountingSystem_WebUI.Models;
using Services;
using System.Threading.Tasks;

namespace HomeAccountingSystem_WebUI.Helpers
{
    public class CategoryHelper : ICategoryHelper
    {
        private ICategoryService _categoryService;

        public CategoryHelper(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<CategoriesViewModel> CreateCategoriesViewModel(int page, int itemsPerPage, Func<Category, bool> predicate)
        {
            var categories = (await _categoryService.GetListAsync())
                    .Where(predicate)
                    .ToList();
            return  new CategoriesViewModel()
            {
                Categories = categories
                    .Skip((page - 1) * itemsPerPage)
                    .Take(itemsPerPage)
                    .ToList(),
                PagingInfo = new PagingInfo()
                {
                    CurrentPage = page,
                    TotalItems = categories.Count(),
                    ItemsPerPage = itemsPerPage
                }
            };
        }
    }
}