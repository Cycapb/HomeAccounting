using DomainModels.Model;
using System;
using System.Collections.Generic;
using WebUI.Models;
using WebUI.Models.CategoryModels;
using WebUI.Models.ReportModels;

namespace WebUI.Abstract
{
    public interface IReportControllerHelper : IDisposable
    {
        void FillReportMonthsModel(ReportMonthsModel model, List<PayingItem> repo);

        IEnumerable<Category> GetCategoriesByType(WebUser user, int flowId);

        IEnumerable<PayingItem> GetPayingItemsForLastYear(WebUser user);

        IEnumerable<CategorySumModel> GetOverallList(WebUser user, DateTime dateFrom, DateTime dateTo, int flowId);

        IEnumerable<PayingItem> GetPayingItemsInDates(DateTime dtFrom, DateTime dtTo, WebUser user);

        IEnumerable<Category> GetActiveCategoriesByType(WebUser user, int flowId);
    }
}
