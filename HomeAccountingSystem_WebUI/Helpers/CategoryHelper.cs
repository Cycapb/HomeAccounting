using System;
using System.Linq;
using DomainModels.Model;
using WebUI.Abstract;
using WebUI.Models;
using Services;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace WebUI.Helpers
{
    public class CategoryHelper : ICategoryHelper
    {
        private ICategoryService _categoryService;

        public CategoryHelper(ICategoryService categoryService) { _categoryService = categoryService; }

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

        public async Task<IEnumerable<Category>> GetCategoriesToShowOnPage(int page, int itemsPerPage, Func<Category, bool> predicate)
        {
            return (await _categoryService.GetListAsync())
                .Where(predicate)
                .Skip((page - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .ToList();
        }
    }
}