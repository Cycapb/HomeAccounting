using System;
using HomeAccountingSystem_WebUI.Models;
using DomainModels.Model;
using System.Threading.Tasks;

namespace HomeAccountingSystem_WebUI.Abstract
{
    public interface ICategoryHelper
    {
        Task<CategoriesViewModel> CreateCategoriesViewModel(int page, int itemsPerPage, Func<Category, bool> predicate);
    }
}
