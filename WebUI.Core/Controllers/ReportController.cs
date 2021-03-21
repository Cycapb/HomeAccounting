using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Paginator.Abstract;
using Services.Exceptions;
using WebUI.Core.Abstract;
using WebUI.Core.Abstract.Helpers;
using WebUI.Core.Exceptions;
using WebUI.Core.Infrastructure.Filters;
using WebUI.Core.Models;
using WebUI.Core.Models.ReportModels;

namespace WebUI.Core.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        private readonly IPayItemSubcategoriesHelper _payItemSubcategoriesHelper;
        private readonly IReportControllerHelper _reportControllerHelper;
        private readonly IReportModelCreator _reportModelCreator;
        private readonly IPageCreator _pageCreator;

        public ReportController(IPayItemSubcategoriesHelper payItemSubcategoriesHelper,
            IReportControllerHelper reportControllerHelper,
            IReportModelCreator reportModelCreator,
            IPageCreator pageCreator)
        {
            _payItemSubcategoriesHelper = payItemSubcategoriesHelper;
            _reportControllerHelper = reportControllerHelper;
            _reportModelCreator = reportModelCreator;
            _pageCreator = pageCreator;
        }

        public IActionResult Index()
        {
            return PartialView("_Index");
        }

        [TypeFilter(typeof(UserHasCategories))]
        public IActionResult CreateByTypeOfFlowView(WebUser user, int typeOfFlowId)
        {
            ViewBag.TypeOfFlowId = typeOfFlowId;

            try
            {
                var items = _reportControllerHelper.GetActiveCategoriesByType(user, typeOfFlowId);

                return PartialView("_CreateByTypeOfFlowView", items);
            }
            catch (ServiceException e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(ReportController)} в методе {nameof(CreateByTypeOfFlowView)}", e);
            }
        }

        public IActionResult GetTypeOfFlowReport(ReportByCategoryAndTypeOfFlowModel model, WebUser user, int page = 1)
        {
            try
            {
                var reportModel = _reportModelCreator.CreateByTypeReportModel(model, user, page);
                ViewBag.PageCreator = _pageCreator;

                return PartialView("_GetByTypeOfFlowReport", reportModel);
            }
            catch (WebUiException e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(ReportController)} в методе {nameof(GetTypeOfFlowReport)}", e);
            }
        }

        public IActionResult CreateByDatesView()
        {
            return PartialView("_CreateByDatesView");
        }

        [TypeFilter(typeof(UserHasCategories))]
        public IActionResult GetByDatesReport(WebUser user, DateTime dtFrom, DateTime dtTo, int page = 1)
        {
            try
            {
                var reportModel = _reportModelCreator.CreateByDatesReportModel(user, dtFrom, dtTo, page);
                ViewBag.OutgoSum = GetSummOfItems(reportModel.AllItems, 2);
                ViewBag.IncomingSum = GetSummOfItems(reportModel.AllItems, 1);
                ViewBag.PageCreator = _pageCreator;

                return PartialView("_GetByDatesReport", reportModel);
            }
            catch (WebUiException e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(ReportController)} в методе {nameof(GetByDatesReport)}",
                    e);
            }
        }

        [TypeFilter(typeof(UserHasCategories))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GetAllCategoriesReport(WebUser user, DateTime dateFrom, DateTime dateTo, int typeOfFlowId)
        {
            ViewBag.TypeOfFlowName = typeOfFlowId == 1 ? "Доход" : "Расход";
            ViewBag.dtFrom = dateFrom;
            ViewBag.dtTo = dateTo;
            try
            {
                var list = _reportControllerHelper.GetOverallList(user, dateFrom, dateTo, typeOfFlowId)
                    .OrderByDescending(x => x.Sum)
                    .ToList();
                ViewBag.Summ = list.Sum(x => x.Sum);

                return PartialView("_GetAllCategoriesReport", list);
            }
            catch (WebUiHelperException e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(ReportController)} в методе {nameof(GetAllCategoriesReport)}",
                    e);
            }
        }

        public IActionResult GetItemsByMonth(WebUser user, DateTime date)
        {
            var dtFrom = date;
            var dtTo = EndDateFromDate(date);

            return RedirectToAction("GetByDatesReport", new { dtFrom, dtTo });
        }

        public async Task<ActionResult> SubcategoriesReport(WebUser user, int typeOfFlowId, DateTime date)
        {
            var payItemSubcategoriesList = await GetPayitemSubcategoriesForView(user, typeOfFlowId, date);

            return PartialView("_SubcategoriesReport", payItemSubcategoriesList);
        }

        protected override void Dispose(bool disposing)
        {
            _payItemSubcategoriesHelper.Dispose();
            _reportControllerHelper.Dispose();
            _reportModelCreator.Dispose();

            base.Dispose(disposing);
        }

        private async Task<List<PayItemSubcategories>> GetPayitemSubcategoriesForView(WebUser user, int typeOfFlowId, DateTime date)
        {
            ViewBag.TypeOfFlowName = typeOfFlowId == 1 ? "Доход" : "Расход";
            ViewBag.Month = date.ToString("MMMMMM", CultureInfo.CurrentCulture);
            var dtTo = EndDateFromDate(date);

            try
            {
                var payItemSubcategoriesList =
                    await _payItemSubcategoriesHelper.GetPayItemsWithSubcategoriesInDatesWeb(date, dtTo, user,
                        typeOfFlowId);
                ViewBag.Summ = payItemSubcategoriesList.Sum(x => x.CategorySumm.Sum);

                return payItemSubcategoriesList;
            }
            catch (WebUiHelperException e)
            {
                throw new WebUiException(
                    $"Ошибка в контроллере {nameof(ReportController)} в методе {nameof(GetPayitemSubcategoriesForView)}",
                    e);
            }
        }

        private DateTime EndDateFromDate(DateTime date)
        {
            var daysInmonth = DateTime.DaysInMonth(date.Year, date.Month);
            return date + TimeSpan.FromDays(daysInmonth - 1);
        }

        private decimal GetSummOfItems(List<PayItem> list, int typeOfFlowId)
        {
            return list
                .Where(x => x.TypeOfFlowId == typeOfFlowId)
                .Sum(x => x.Summ);

        }
    }
}