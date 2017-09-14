using System;
using System.Linq;
using DomainModels.Model;
using WebUI.Abstract;
using WebUI.Models;
using Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using Services.Exceptions;
using WebUI.Controllers;
using WebUI.Exceptions;

namespace WebUI.Helpers
{
    public class CategoryHelper : ICategoryHelper
    {
        private readonly ICategoryService _categoryService;

        public CategoryHelper(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<CategoriesViewModel> CreateCategoriesViewModel(int page, int itemsPerPage,
            Func<Category, bool> predicate)
        {
            List<Category> categories = new List<Category>();
            try
            {
                categories = (await _categoryService.GetListAsync())
                    .Where(predicate)
                    .ToList();
            }
            catch (ServiceException e)
            {
                throw new WebUiException(
                    $"Ошибка в контроллере {nameof(CategoryHelper)} в методе {nameof(CreateCategoriesViewModel)}", e);
            }

            return new CategoriesViewModel()
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

        public async Task<IEnumerable<Category>> GetCategoriesToShowOnPage(int page, int itemsPerPage,
            Func<Category, bool> predicate)
        {
            try
            {
                return (await _categoryService.GetListAsync())
                    .Where(predicate)
                    .Skip((page - 1) * itemsPerPage)
                    .Take(itemsPerPage)
                    .ToList();
            }
            catch (ServiceException e)
            {
                throw new WebUiException(
                    $"Ошибка в контроллере {nameof(CategoryHelper)} в методе {nameof(GetCategoriesToShowOnPage)}", e);
            }
        }
    }
}