using System;
using WebUI.Core.Models.CategoryModels;
using DomainModels.Model;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace WebUI.Core.Abstract
{
    public interface ICategoryHelper : IDisposable
    {
        Task<CategoriesCollectionModel> CreateCategoriesViewModel(int page, int itemsPerPage, Expression<Func<Category, bool>> predicate);

        Task<IEnumerable<Category>> GetCategoriesToShowOnPage(int page, int itemsPerPage, Expression<Func<Category, bool>> predicate);
    }
}
