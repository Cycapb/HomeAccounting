using DomainModels.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebUI.Models.CategoryModels;

namespace WebUI.Abstract
{
    public interface ICategoryHelper : IDisposable
    {
        Task<CategoriesCollectionModel> CreateCategoriesViewModel(int page, int itemsPerPage, Func<Category, bool> predicate);

        Task<IEnumerable<Category>> GetCategoriesToShowOnPage(int page, int itemsPerPage, Func<Category, bool> predicate);
    }
}
