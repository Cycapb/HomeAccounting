using System;
using System.Linq;
using DomainModels.Model;
using Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using Services.Exceptions;
using WebUI.Core.Abstract;
using WebUI.Core.Models.CategoryModels;
using WebUI.Core.Exceptions;
using WebUI.Core.Models;
using System.Linq.Expressions;

namespace WebUI.Core.Helpers
{
    public class CategoryHelper : ICategoryHelper
    {
        private readonly ICategoryService _categoryService;
        private bool _disposed;

        public CategoryHelper(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<CategoriesCollectionModel> CreateCategoriesViewModel(
            int page, 
            int itemsPerPage,
            Expression<Func<Category, bool>> predicate)
        {
            try
            {
                var categories = (await _categoryService.GetListAsync(predicate)).ToList();

                return new CategoriesCollectionModel()
                {
                    Categories = categories
                        .Skip((page - 1) * itemsPerPage)
                        .Take(itemsPerPage)
                        .ToList(),
                    PagingInfo = new PagingInfo()
                    {
                        CurrentPage = page,
                        TotalItems = categories.Count,
                        ItemsPerPage = itemsPerPage
                    }
                };
            }
            catch (ServiceException e)
            {
                throw new WebUiHelperException(
                    $"Ошибка в типе {nameof(CategoryHelper)} в методе {nameof(CreateCategoriesViewModel)}", e);
            }
            catch (Exception e)
            {
                throw new WebUiHelperException(
                    $"Ошибка {e.GetType()} в типе {nameof(CategoryHelper)} в методе {nameof(CreateCategoriesViewModel)}", e);
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _categoryService.Dispose();
                }

                _disposed = true;
            }
        }

        public async Task<IEnumerable<Category>> GetCategoriesToShowOnPage(
            int page, 
            int itemsPerPage,
            Expression<Func<Category, bool>> predicate)
        {
            try
            {
                return (await _categoryService.GetListAsync(predicate))
                    .Skip((page - 1) * itemsPerPage)
                    .Take(itemsPerPage)
                    .ToList();
            }
            catch (ServiceException e)
            {
                throw new WebUiHelperException(
                    $"Ошибка в типе {nameof(CategoryHelper)} в методе {nameof(GetCategoriesToShowOnPage)}", e);
            }
            catch (Exception e)
            {
                throw new WebUiHelperException(
                    $"Ошибка {e.GetType()} в типе {nameof(CategoryHelper)} в методе {nameof(GetCategoriesToShowOnPage)}", e);
            }
        }
    }
}