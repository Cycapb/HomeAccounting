using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DomainModels.Abstract;
using HomeAccountingSystem_WebUI.Models;

namespace HomeAccountingSystem_WebUI.Abstract
{
    public interface IPayItemSubcategoriesHelper
    {
        Task<List<PayItemSubcategories>> GetPayItemsWithSubcategoriesInDatesWeb(DateTime dateFrom, DateTime dateTo,
            IWorkingUser user, int typeOfFlowId);
    }
}
