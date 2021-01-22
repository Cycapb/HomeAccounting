using DomainModels.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebUI.Core.Models.ReportModels;

namespace WebUI.Core.Abstract.Helpers
{
    public interface IReportHelper : IDisposable
    {
        IEnumerable<PayingItem> GetPayingItemsInDatesWeb(DateTime dateFrom, DateTime dateTo, IWorkingUser user);
        IEnumerable<PayingItem> GetPayingItemsByDateWeb(DateTime date, IWorkingUser user);
        IEnumerable<PayItem> GetPayItemsByDateWeb(DateTime date, IWorkingUser user);
        IEnumerable<PayItem> GetPayItemsInDatesWeb(DateTime dateFrom, DateTime dateTo, IWorkingUser user);
        IEnumerable<PayItem> GetCategoryPayItemsByDateWeb(DateTime date, int categoryId, IWorkingUser user);

        IEnumerable<PayItem> GetCategoryPayItemsInDatesWeb(DateTime dateFrom, DateTime dateTo, int categoryId,
            IWorkingUser user);

        IEnumerable<PayItem> GetPayItemsInDatesByTypeOfFlowWeb(DateTime dateFrom, DateTime dateTo,
            int typeOfFlowId, IWorkingUser user);

        Task<string> GetBudgetOverAllAsync(IWorkingUser user);
        Task<string> GetBudgetInFactAsync(IWorkingUser user);

        decimal GetSummForMonth(List<PayingItem> collection);
        decimal GetSummForWeek(List<PayingItem> collection);
        decimal GetSummForDay(List<PayingItem> collection);
    }
}
