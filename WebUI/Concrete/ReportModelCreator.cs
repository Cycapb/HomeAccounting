using Services;
using Services.Caching;
using Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using WebUI.Abstract;
using WebUI.Exceptions;
using WebUI.Models;
using WebUI.Models.ReportModels;

namespace WebUI.Concrete
{
    public class ReportModelCreator : IReportModelCreator
    {
        private readonly ICategoryService _categoryService;
        private readonly IReportHelper _dbHelper;
        private readonly IPagingInfoCreator _pagingCreator;
        private readonly int _itemsPerPage = 15;
        private readonly ICacheManager _cacheManager;
        private bool _disposed;

        public ReportModelCreator(
            ICategoryService categoryService,
            IReportHelper dbHelper,
            IPagingInfoCreator pagingCreator,
            ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
            _categoryService = categoryService;
            _dbHelper = dbHelper;
            _pagingCreator = pagingCreator;
        }

        public ReportModel CreateByDatesReportModel(WebUser user, DateTime dtFrom, DateTime dtTo, int page)
        {
            List<PayItem> tempList = new List<PayItem>();
            var cacheKey = "ByDates_" + dtFrom.Date.ToShortDateString() + "_" + dtTo.Date.ToShortDateString();
            try
            {
                var cachedItems = (List<PayItem>)_cacheManager.Get(cacheKey);
                if (cachedItems != null)
                {
                    tempList = cachedItems;
                }
                else
                {
                    tempList = _dbHelper.GetPayItemsInDatesWeb(dtFrom, dtTo, user).OrderByDescending(x => x.Date).ToList();
                    _cacheManager.Set(cacheKey, tempList);
                }
            }
            catch (ServiceException e)
            {
                throw new WebUiException(
                    $"Ошибка в типе {nameof(ReportModelCreator)} в методе {nameof(CreateByDatesReportModel)}",
                    e);
            }
            var reportModel = GetByDatesReportModel(user, dtFrom, dtTo, page, tempList);
            return reportModel;
        }

        public ReportModel CreateByTypeReportModel(ReportByCategoryAndTypeOfFlowModel model, WebUser user, int page)
        {
            List<PayItem> tempList = new List<PayItem>();
            var cacheKey = $"ByTypeOfFlow_{model.TypeOfFlowId}_{model.CatId}_{model.DtFrom.Date.ToShortDateString()}_{model.DtTo.Date.ToShortDateString()}";
            try
            {
                var cachedItems = (List<PayItem>)_cacheManager.Get(cacheKey);
                if (cachedItems != null)
                {
                    tempList = cachedItems;
                }
                else
                {
                    tempList =
                    _dbHelper.GetCategoryPayItemsInDatesWeb(model.DtFrom, model.DtTo, model.CatId, user)
                        .OrderByDescending(x => x.Date)
                        .ToList();
                    _cacheManager.Set(cacheKey, tempList);
                }
            }
            catch (WebUiHelperException e)
            {
                throw new WebUiException(
                    $"Ошибка в типе {nameof(ReportModelCreator)} в методе {nameof(CreateByTypeReportModel)}",
                    e);
            }
            var reportModel = GetByTypeOfFlowReportModel(model, user, page, tempList);
            return reportModel;
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
                }

                _disposed = true;
            }
        }

        private ReportModel GetByTypeOfFlowReportModel(ReportByCategoryAndTypeOfFlowModel model, WebUser user, int page,
            List<PayItem> pItemList)
        {
            string categoryName;
            try
            {
                categoryName = _categoryService.GetItem(model.CatId).Name;
            }
            catch (ServiceException e)
            {
                throw new WebUiException(
                    $"Ошибка в типе {nameof(ReportModelCreator)} в методе {nameof(GetByTypeOfFlowReportModel)}",
                    e);
            }
            return new ReportModel()
            {
                CategoryName = categoryName,
                ItemsPerPage = pItemList
                    .Skip((page - 1) * _itemsPerPage)
                    .Take(_itemsPerPage)
                    .ToList(),
                AllItems = pItemList.ToList(),
                PagingInfo = _pagingCreator.Create(page, _itemsPerPage, pItemList.Count),
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