using DomainModels.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebUI.Models.ReportModels;

namespace WebUI.Abstract
{
    public interface IPayItemSubcategoriesHelper : IDisposable
    {
        Task<List<PayItemSubcategories>> GetPayItemsWithSubcategoriesInDatesWeb(DateTime dateFrom, DateTime dateTo,
            IWorkingUser user, int typeOfFlowId);
    }
}
