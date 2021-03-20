using DomainModels.Model;
using Microsoft.EntityFrameworkCore;
using Services;
using Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WebUI.Core.Abstract.Helpers;
using WebUI.Core.Exceptions;
using WebUI.Core.Models;
using WebUI.Core.Models.CategoryModels;
using WebUI.Core.Models.ReportModels;

namespace WebUI.Core.Implementations.Helpers
{
    public class ReportControllerHelper : IReportControllerHelper
    {
        private readonly ICategoryService _categoryService;
        private readonly IPayingItemService _payingItemService;
        private readonly IReportHelper _dbHelper;
        private bool _disposed;

        public ReportControllerHelper(ICategoryService categoryService, IPayingItemService payingItemService,
            IReportHelper dbHelper)
        {
            _categoryService = categoryService;
            _payingItemService = payingItemService;
            _dbHelper = dbHelper;
        }

        public IEnumerable<Category> GetCategoriesByType(WebUser user, int flowId)
        {
            try
            {
                return _categoryService.GetList(x => x.TypeOfFlowID == flowId && x.UserId == user.Id)
                    .OrderBy(x => x.Name)
                    .ToList();
            }
            catch (ServiceException e)
            {
                throw new WebUiHelperException(
                    $"Ошибка в типе {nameof(ReportControllerHelper)} в методе {nameof(GetCategoriesByType)}", e);
            }
        }

        public IEnumerable<PayingItem> GetPayingItemsForLastYear(WebUser user)
        {
            var dateFrom = DateTime.Parse(DateTime.Today.ToString("Y", CultureInfo.CurrentCulture));

            try
            {
                return _payingItemService.GetList(x => x.UserId == user.Id && x.Date <= DateTime.Today.AddMonths(1) && x.Date >= DateTime.Today.AddYears(-1))                
                    .ToList();
            }
            catch (ServiceException e)
            {
                throw new WebUiHelperException(
                    $"Ошибка в типе {nameof(ReportControllerHelper)} в методе {nameof(GetPayingItemsForLastYear)}", e);
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

        public IEnumerable<CategorySumModel> GetOverallList(WebUser user, DateTime dateFrom, DateTime dateTo, int flowId)
        {
            IEnumerable<PayItem> pItemList = new List<PayItem>();
            try
            {
                pItemList = _dbHelper.GetPayItemsInDatesByTypeOfFlowWeb(dateFrom, dateTo, flowId, user);
            }
            catch (WebUiHelperException e)
            {
                throw new WebUiHelperException(
                    $"Ошибка в типе {nameof(ReportControllerHelper)} в методе {nameof(GetOverallList)}", e);
            }
            return (from l in pItemList
                    group l by l.CategoryName
                    into grouping
                    select new CategorySumModel()
                    {
                        Category = grouping.Key,
                        Sum = grouping.Sum(s => s.Summ)
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
                    $"Ошибка в типе {nameof(ReportControllerHelper)} в методе {nameof(GetPayingItemsInDates)}", e);
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
            foreach (var item in listMonths)
            {
                model.MonthInOuts.Add(new MonthInOut()
                {
                    Month = item.Date,
                    Date = DateTime.Parse(item.Date)
                });
            }
        }

        private void FillIncomeSumm(List<MonthSumm> listmonths, ReportMonthsModel model)
        {
            foreach (var monthSumm in listmonths)
            {
                foreach (var monthInOut in model.MonthInOuts)
                {
                    if (monthInOut.Month != monthSumm.Date) continue;
                    monthInOut.SummIn = monthSumm.Summ.ToString("C");
                    break;
                }
            }
        }

        private void FillOutgoSumm(List<MonthSumm> listmonths, ReportMonthsModel model)
        {
            foreach (var monthSumm in listmonths)
            {
                foreach (var monthInOut in model.MonthInOuts)
                {
                    if (monthInOut.Month != monthSumm.Date) continue;
                    monthInOut.SummOut = monthSumm.Summ.ToString("C");
                    break;
                }
            }
        }

        public IEnumerable<Category> GetActiveCategoriesByType(WebUser user, int flowId)
        {
            try
            {
                return _categoryService.GetList(c => c.UserId == user.Id && c.Active && c.TypeOfFlowID == flowId)
                .OrderBy(c => c.Name)
                .ToList();
            }
            catch (ServiceException e)
            {
                throw new WebUiHelperException(
                    $"Ошибка в типе {nameof(ReportControllerHelper)} в методе {nameof(GetActiveCategoriesByType)}", e);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _categoryService.Dispose();
                    _dbHelper.Dispose();
                    _payingItemService.Dispose();
                }

                _disposed = true;
            }
        }
    }
}