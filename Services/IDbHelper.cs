using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DomainModels.Model;

namespace Services
{
    public interface IDbHelper : IDisposable
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

        Task<string> GetBudgetOverAllWeb(IWorkingUser user);
        Task<string> GetBudgetInFactWeb(IWorkingUser user);

        decimal GetSummForMonth(List<PayingItem> collection);
        decimal GetSummForWeek(List<PayingItem> collection);
        decimal GetSummForDay(List<PayingItem> collection);
    }
}
