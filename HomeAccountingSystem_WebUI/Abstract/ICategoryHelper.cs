using System;
using HomeAccountingSystem_WebUI.Models;
using DomainModels.Model;

namespace HomeAccountingSystem_WebUI.Abstract
{
    public interface ICategoryHelper
    {
        CategoriesViewModel CreateCategoriesViewModel(int page, int itemsPerPage, Action<Category> cat);
    }
}
