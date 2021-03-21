using System;
using System.Collections.Generic;
using DomainModels.Model;
using WebUI.Core.Models;
using WebUI.Core.Models.CategoryModels;
using WebUI.Core.Models.ReportModels;

namespace WebUI.Core.Abstract.Helpers
{
    public interface IReportControllerHelper : IDisposable
    {
        void FillReportMonthsModel(ReportOverallLastYearByMonthsModel model, List<PayingItem> repo);

        IEnumerable<Category> GetCategoriesByType(WebUser user, int flowId);

        IEnumerable<PayingItem> GetPayingItemsForLastYear(WebUser user);

        IEnumerable<CategorySumModel> GetOverallList(WebUser user, DateTime dateFrom, DateTime dateTo, int flowId);

        IEnumerable<PayingItem> GetPayingItemsInDates(DateTime dtFrom, DateTime dtTo, WebUser user);

        IEnumerable<Category> GetActiveCategoriesByType(WebUser user, int flowId);
    }
}
