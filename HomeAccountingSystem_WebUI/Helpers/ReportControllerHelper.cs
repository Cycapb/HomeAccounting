using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DomainModels.Model;
using DomainModels.Repositories;
using HomeAccountingSystem_WebUI.Abstract;
using HomeAccountingSystem_WebUI.Models;
using Services;
using WebGrease.Css.Extensions;

namespace HomeAccountingSystem_WebUI.Helpers
{
    public class ReportControllerHelper:IReportControllerHelper
    {
        private readonly IRepository<Category> _catRepository;
        private readonly IRepository<PayingItem> _pItemRepository;
        private readonly IDbHelper _dbHelper;

        public ReportControllerHelper(IRepository<Category> catRepository, IRepository<PayingItem> pItemRepository,
            IDbHelper dbHelper)
        {
            _catRepository = catRepository;
            _pItemRepository = pItemRepository;
            _dbHelper = dbHelper;
        }

        public IEnumerable<Category> GetCategoriesByType(WebUser user, int flowId)
        {
           return _catRepository.GetList()
                .Where(x => x.TypeOfFlowID == flowId && x.UserId == user.Id)
                .ToList();
        }

        public IEnumerable<PayingItem> GetPayingItemsForLastYear(WebUser user)
        {
            return _pItemRepository.GetList()
                .Where(x => x.UserId == user.Id &&
                            ((x.Date <= DateTime.Today) && (x.Date >= DateTime.Today.AddYears(-1))))
                .ToList();
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
            var pItemList = _dbHelper.GetPayItemsInDatesByTypeOfFlowWeb(dateFrom, dateTo, flowId, user);
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
            return _dbHelper.GetPayingItemsInDatesWeb(dtFrom, dtTo, user);
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