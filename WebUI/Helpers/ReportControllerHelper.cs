using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DomainModels.Model;
using WebUI.Abstract;
using WebUI.Models;
using Services;
using Services.Exceptions;
using WebGrease.Css.Extensions;
using WebUI.Exceptions;

namespace WebUI.Helpers
{
    public class ReportControllerHelper:IReportControllerHelper
    {
        private readonly ICategoryService _categoryService;
        private readonly IPayingItemService _payingItemService;
        private readonly IDbHelper _dbHelper;

        public ReportControllerHelper(ICategoryService categoryService, IPayingItemService payingItemService,
            IDbHelper dbHelper)
        {
            _categoryService = categoryService;
            _payingItemService = payingItemService;
            _dbHelper = dbHelper;
        }

        public IEnumerable<Category> GetCategoriesByType(WebUser user, int flowId)
        {
            try
            {
                return _categoryService.GetList()
                    .Where(x => x.TypeOfFlowID == flowId && x.UserId == user.Id)
                    .ToList();
            }
            catch (ServiceException e)
            {
                throw new WebUiHelperException(
                    $"Ошибка в типе {nameof(PayingItemHelper)} в методе {nameof(GetCategoriesByType)}", e);
            }
        }

        public IEnumerable<PayingItem> GetPayingItemsForLastYear(WebUser user)
        {
            try
            {
                return _payingItemService.GetList()
                    .Where(x => x.UserId == user.Id && x.Date <= DateTime.Today && x.Date >= DateTime.Today.AddYears(-1))
                    .ToList();
            }
            catch (ServiceException e)
            {
                throw new WebUiHelperException(
                    $"Ошибка в типе {nameof(PayingItemHelper)} в методе {nameof(GetPayingItemsForLastYear)}", e);
            }
        }

        public void FillReportMonthsModel(ReportMonthsModel model, List<PayingItem> repo)
        {
            var income = GetOverallByTypeOfFlow(repo, 1);
            var outgo = GetOverallByTypeOfFlow(repo, 2);
            model.MonthInOuts = new List<MonthInOut>();

            if (income.Count > outgo.Count)
            {
                FillMonths(income, model);
                FillIncomeSumm(income, model);
                FillOutgoSumm(outgo, model);
            }
            else
            {
                FillMonths(outgo, model);
                FillIncomeSumm(income, model);
                FillOutgoSumm(outgo, model);
            }
            model.MonthInOuts.Reverse();
        }

        public IEnumerable<OverAllItem> GetOverallList(WebUser user, DateTime dateFrom, DateTime dateTo, int flowId)
        {
            IEnumerable<PayItem> pItemList = new List<PayItem>();
            try
            {
                pItemList = _dbHelper.GetPayItemsInDatesByTypeOfFlowWeb(dateFrom, dateTo, flowId, user);
            }
            catch (WebUiHelperException e)
            {
                throw new WebUiHelperException(
                    $"Ошибка в типе {nameof(PayingItemHelper)} в методе {nameof(GetOverallList)}", e);
            }
            return (from l in pItemList
                    group l by l.CategoryName
                    into grouping
                    select new OverAllItem()
                    {
                        Category = grouping.Key,
                        Summ = grouping.Sum(s => s.Summ)
                    })
                .ToList();
        }

        public IEnumerable<PayingItem> GetPayingItemsInDates(DateTime dtFrom, DateTime dtTo, WebUser user)
        {
            try
            {
                return _dbHelper.GetPayingItemsInDatesWeb(dtFrom, dtTo, user);
            }
            catch (WebUiHelperException e)
            {
                throw new WebUiHelperException(
                    $"Ошибка в типе {nameof(PayingItemHelper)} в методе {nameof(GetPayingItemsInDates)}", e);
            }
        }

        private List<MonthSumm> GetOverallByTypeOfFlow(IEnumerable<PayingItem> repo, int id)
        {
                return repo.Where(x => x.Category.TypeOfFlowID == id)
                .GroupBy(x => x.Date.ToString("Y", CultureInfo.CurrentCulture))
                .Select(x => new MonthSumm()
                {
                    Date = x.Key,
                    Summ = x.Sum(y => y.Summ)
                })
                .ToList();
        }

        private void FillMonths(IEnumerable<MonthSumm> listMonths, ReportMonthsModel model)
        {
            listMonths.ForEach(x=>model.MonthInOuts.Add(new MonthInOut()
            {
                Month = x.Date,
                Date = DateTime.Parse(x.Date)
            }));
        }

        private void FillIncomeSumm(List<MonthSumm> listmonths, ReportMonthsModel model)
        {
            for (int i = 0; i < listmonths.Count; i++)
            {
                model.MonthInOuts[i].SummIn = listmonths[i].Summ.ToString("c");
            }
        }

        private void FillOutgoSumm(List<MonthSumm> listmonths, ReportMonthsModel model)
        {
            for (int i = 0; i < listmonths.Count; i++)
            {
                model.MonthInOuts[i].SummOut = listmonths[i].Summ.ToString("c");
            }
        }
    }
}