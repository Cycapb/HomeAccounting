using DomainModels.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebUI.Core.Models.ReportModels;

namespace WebUI.Core.Abstract.Helpers
{
    public interface IReportHelper : IDisposable
    {
        IEnumerable<PayingItem> GetPayingItemsInDatesWeb(DateTime dateFrom, DateTime dateTo, string userId);
        IEnumerable<PayingItem> GetPayingItemsByDateWeb(DateTime date, string userId);
        IEnumerable<PayItem> GetPayItemsByDateWeb(DateTime date, string userId);
        IEnumerable<PayItem> GetPayItemsInDatesWeb(DateTime dateFrom, DateTime dateTo, string userId);
        IEnumerable<PayItem> GetCategoryPayItemsByDateWeb(DateTime date, int categoryId, string userId);

        IEnumerable<PayItem> GetCategoryPayItemsInDatesWeb(DateTime dateFrom, DateTime dateTo, int categoryId,
            string userId);

        IEnumerable<PayItem> GetPayItemsInDatesByTypeOfFlowWeb(DateTime dateFrom, DateTime dateTo,
            int typeOfFlowId, string userId);

        Task<string> GetBudgetOverAllAsync(string userId);
        Task<string> GetBudgetInFactAsync(string userId);

        decimal GetSummForMonth(List<PayingItem> collection);
        decimal GetSummForWeek(List<PayingItem> collection);
        decimal GetSummForDay(List<PayingItem> collection);
    }
}
