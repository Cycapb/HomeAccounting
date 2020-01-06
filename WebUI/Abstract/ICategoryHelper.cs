using System;
using WebUI.Models;
using DomainModels.Model;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace WebUI.Abstract
{
    public interface ICategoryHelper : IDisposable
    {
        Task<CategoriesViewModel> CreateCategoriesViewModel(int page, int itemsPerPage, Func<Category, bool> predicate);
        Task<IEnumerable<Category>> GetCategoriesToShowOnPage(int page, int itemsPerPage, Func<Category, bool> predicate);
    }
}
