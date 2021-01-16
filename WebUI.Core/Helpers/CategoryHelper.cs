using DomainModels.Model;
using Services;
using Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebUI.Core.Abstract.Helpers;
using WebUI.Core.Exceptions;
using WebUI.Core.Models;
using WebUI.Core.Models.CategoryModels;

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

        public async Task<CategoriesCollectionModel> CreateCategoriesViewModelAsync(
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
                    $"Ошибка в типе {nameof(CategoryHelper)} в методе {nameof(CreateCategoriesViewModelAsync)}", e);
            }
            catch (Exception e)
            {
                throw new WebUiHelperException(
                    $"Ошибка {e.GetType()} в типе {nameof(CategoryHelper)} в методе {nameof(CreateCategoriesViewModelAsync)}", e);
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task<IEnumerable<Category>> GetCategoriesToShowOnPageAsync(
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
                    $"Ошибка в типе {nameof(CategoryHelper)} в методе {nameof(GetCategoriesToShowOnPageAsync)}", e);
            }
            catch (Exception e)
            {
                throw new WebUiHelperException(
                    $"Ошибка {e.GetType()} в типе {nameof(CategoryHelper)} в методе {nameof(GetCategoriesToShowOnPageAsync)}", e);
            }
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
    }
}