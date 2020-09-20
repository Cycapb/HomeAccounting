using DomainModels.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebUI.Core.Models.ReportModels;

namespace WebUI.Core.Abstract
{
    public interface IPayItemSubcategoriesHelper : IDisposable
    {
        Task<List<PayItemSubcategories>> GetPayItemsWithSubcategoriesInDatesWeb(
            DateTime dateFrom,
            DateTime dateTo,
            IWorkingUser user,
            int typeOfFlowId);
    }
}
