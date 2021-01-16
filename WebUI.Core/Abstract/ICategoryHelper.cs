using DomainModels.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebUI.Core.Models.CategoryModels;

namespace WebUI.Core.Abstract
{
    public interface ICategoryHelper : IDisposable
    {
        Task<CategoriesCollectionModel> CreateCategoriesViewModelAsync(int page, int itemsPerPage, Expression<Func<Category, bool>> predicate);

        Task<IEnumerable<Category>> GetCategoriesToShowOnPageAsync(int page, int itemsPerPage, Expression<Func<Category, bool>> predicate);
    }
}
