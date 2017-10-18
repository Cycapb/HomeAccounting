using System;
using System.Collections.Generic;
using DomainModels.Model;
using WebUI.Models;

namespace WebUI.Abstract
{
    public interface IReportControllerHelper
    {
        void FillReportMonthsModel(ReportMonthsModel model, List<PayingItem> repo);
        IEnumerable<Category> GetCategoriesByType(WebUser user, int flowId);
        IEnumerable<PayingItem> GetPayingItemsForLastYear(WebUser user);
        IEnumerable<OverAllItem> GetOverallList(WebUser user, DateTime dateFrom, DateTime dateTo, int flowId);
        IEnumerable<PayingItem> GetPayingItemsInDates(DateTime dtFrom, DateTime dtTo, WebUser user);
    }
}
