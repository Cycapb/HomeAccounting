using Microsoft.Extensions.Caching.Memory;
using Services;
using Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using WebUI.Core.Abstract;
using WebUI.Core.Abstract.Helpers;
using WebUI.Core.Exceptions;
using WebUI.Core.Models;
using WebUI.Core.Models.ReportModels;

namespace WebUI.Core.Implementations
{
    public class ReportModelCreator : IReportModelCreator
    {
        private readonly ICategoryService _categoryService;
        private readonly IReportHelper _reportHelper;
        private readonly IPagingInfoCreator _pagingCreator;
        private readonly int _itemsPerPage = 15;
        private readonly IMemoryCache _cache;
        private bool _disposed;

        public ReportModelCreator(
            ICategoryService categoryService,
            IReportHelper dbHelper,
            IPagingInfoCreator pagingCreator,
            IMemoryCache cache)
        {
            _cache = cache;
            _categoryService = categoryService;
            _reportHelper = dbHelper;
            _pagingCreator = pagingCreator;
        }

        public ReportModel CreateByDatesReportModel(WebUser user, DateTime dtFrom, DateTime dtTo, int page)
        {
            var cacheKey = "ByDates_" + dtFrom.Date.ToShortDateString() + "_" + dtTo.Date.ToShortDateString();

            try
            {
                var cachedItems = (List<PayItem>)_cache.Get(cacheKey);

                List<PayItem> tempList;

                if (cachedItems != null)
                {
                    tempList = cachedItems;
                }
                else
                {
                    tempList = _reportHelper.GetPayItemsInDatesWeb(dtFrom, dtTo, user.Id).OrderByDescending(x => x.Date).ToList();
                    _cache.Set(cacheKey, tempList);
                }

                var reportModel = GetByDatesReportModel(dtFrom, dtTo, page, tempList);

                return reportModel;
            }
            catch (ServiceException e)
            {
                throw new WebUiException(
                    $"Ошибка в типе {nameof(ReportModelCreator)} в методе {nameof(CreateByDatesReportModel)}",
                    e);
            }
        }

        public ReportModel CreateByTypeReportModel(ReportByCategoryAndTypeOfFlowModel model, WebUser user, int page)
        {
            var cacheKey = $"ByTypeOfFlow_{model.TypeOfFlowId}_{model.CategoryId}_{model.DtFrom.Date.ToShortDateString()}_{model.DtTo.Date.ToShortDateString()}";

            try
            {
                var cachedItems = (List<PayItem>)_cache.Get(cacheKey);
                List<PayItem> tempList;

                if (cachedItems != null)
                {
                    tempList = cachedItems;
                }
                else
                {
                    tempList =
                    _reportHelper.GetCategoryPayItemsInDatesWeb(model.DtFrom, model.DtTo, model.CategoryId, user.Id)
                        .OrderByDescending(x => x.Date)
                        .ToList();
                    _cache.Set(cacheKey, tempList);
                }

                var reportModel = GetByTypeOfFlowReportModel(model, page, tempList);

                return reportModel;
            }
            catch (WebUiHelperException e)
            {
                throw new WebUiException(
                    $"Ошибка в типе {nameof(ReportModelCreator)} в методе {nameof(CreateByTypeReportModel)}",
                    e);
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
                    _reportHelper.Dispose();
                }

                _disposed = true;
            }
        }

        private ReportModel GetByTypeOfFlowReportModel(ReportByCategoryAndTypeOfFlowModel model, int page, List<PayItem> pItemList)
        {
            string categoryName;

            try
            {
                categoryName = _categoryService.GetItem(model.CategoryId).Name;

                return new ReportModel()
                {
                    CategoryName = categoryName,
                    ItemsPerPage = pItemList
                    .Skip((page - 1) * _itemsPerPage)
                    .Take(_itemsPerPage)
                    .ToList(),
                    AllItems = pItemList.ToList(),
                    PagingInfo = _pagingCreator.Create(page, _itemsPerPage, pItemList.Count),
                    CategoryId = model.CategoryId,
                    DtFrom = model.DtFrom,
                    DtTo = model.DtTo,
                    TypeOfFlowId = model.TypeOfFlowId
                };
            }
            catch (ServiceException e)
            {
                throw new WebUiException(
                    $"Ошибка в типе {nameof(ReportModelCreator)} в методе {nameof(GetByTypeOfFlowReportModel)}",
                    e);
            }
        }

        private ReportModel GetByDatesReportModel(DateTime dtFrom, DateTime dtTo, int page, List<PayItem> pItemList)
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