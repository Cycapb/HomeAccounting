using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebUI.Core.Models.ReportModels;

namespace WebUI.Core.Abstract.Helpers
{
    public interface IPayItemSubcategoriesHelper : IDisposable
    {
        Task<List<PayItemSubcategories>> GetPayItemsWithSubcategoriesInDatesWeb(
            DateTime dateFrom,
            DateTime dateTo,
            string userId,
            int typeOfFlowId);
    }
}
