using System;
using System.Collections.Generic;
using System.Linq;
using DomainModels.Model;
using WebUI.Abstract;
using WebUI.Models;
using Services;

namespace WebUI.Concrete
{
    public class ReportModelCreator:IReportModelCreator
    {
        private readonly ICategoryService _categoryService;
        private readonly IDbHelper _dbHelper;
        private readonly IPagingInfoCreator _pagingCreator;
        private readonly int _itemsPerPage = 15;

        public ReportModelCreator(ICategoryService categoryService, IDbHelper dbHelper, IPagingInfoCreator pagingCreator)
        {
            _categoryService = categoryService;
            _dbHelper = dbHelper;
            _pagingCreator = pagingCreator;
        }

        public ReportModel CreateByDatesReportModel(WebUser user, DateTime dtFrom, DateTime dtTo, int page)
        {
            var tempList = _dbHelper.GetPayItemsInDatesWeb(dtFrom, dtTo, user).OrderByDescending(x => x.Date).ToList();
            var reportModel = GetByDatesReportModel(user, dtFrom, dtTo, page, tempList);
            return reportModel;
        }

        public ReportModel CreateByTypeReportModel(TempReportModel model, WebUser user, int page)
        {
            var tempList =
                    _dbHelper.GetCategoryPayItemsInDatesWeb(model.DtFrom, model.DtTo, model.CatId, user)
                        .OrderByDescending(x => x.Date)
                        .ToList();
            var reportModel = GetByTypeOfFlowReportModel(model, user, page, tempList);
            return reportModel;
        }

        private ReportModel GetByTypeOfFlowReportModel(TempReportModel model, WebUser user, int page,
            List<PayItem> pItemList)
        {
            return new ReportModel()
            {
                CategoryName = _categoryService.GetItem(model.CatId).Name,
                ItemsPerPage = pItemList
                    .Skip((page - 1) * _itemsPerPage)
                    .Take(_itemsPerPage)
                    .ToList(),
                AllItems = pItemList.ToList(),
                PagingInfo = _pagingCreator.Create(page,_itemsPerPage,pItemList.Count),
                CategoryId = model.CatId,
                DtFrom = model.DtFrom,
                DtTo = model.DtTo
            };
        }

        private ReportModel GetByDatesReportModel(WebUser user, DateTime dtFrom, DateTime dtTo, int page,
            List<PayItem> pItemList)
        {
            return new ReportModel()
            {
                ItemsPerPage = pItemList
                    .Skip((page - 1) * _itemsPerPage)
                    .Take(_itemsPerPage)
                    .ToList(),
                AllItems = pItemList.ToList(),
                DtFrom = dtFrom,
                DtTo = dtTo,
                PagingInfo = _pagingCreator.Create(page, _itemsPerPage, pItemList.Count)
            };
        }

    }
}