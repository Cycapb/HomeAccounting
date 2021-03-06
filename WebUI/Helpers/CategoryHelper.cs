﻿using System;
using System.Linq;
using DomainModels.Model;
using WebUI.Abstract;
using WebUI.Models;
using Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using Services.Exceptions;
using WebUI.Exceptions;
using WebUI.Models.CategoryModels;

namespace WebUI.Helpers
{
    public class CategoryHelper : ICategoryHelper
    {
        private readonly ICategoryService _categoryService;
        private bool _disposed;

        public CategoryHelper(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<CategoriesCollectionModel> CreateCategoriesViewModel(int page, int itemsPerPage,
            Func<Category, bool> predicate)
        {
            List<Category> categories;
            try
            {
                categories = (await _categoryService.GetListAsync())
                    .Where(predicate)
                    .ToList();
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

            return new CategoriesCollectionModel()
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