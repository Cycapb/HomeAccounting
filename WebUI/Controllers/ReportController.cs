using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.SessionState;
using DomainModels.Model;
using Paginator.Abstract;
using Services.Exceptions;
using WebUI.Abstract;
using WebUI.Exceptions;
using WebUI.Models;
using WebUI.Infrastructure.Attributes;

namespace WebUI.Controllers
{
    [Authorize]
    [SessionState(SessionStateBehavior.ReadOnly)]
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

        public ActionResult Index()
        {
            return PartialView("_Index");
        }

        public ActionResult CreateByTypeOfFlowView(WebUser user, int id)
        {
            ViewBag.TypeOfFlowId = id;
            try
            {
                var items = _reportControllerHelper.GetActiveCategoriesByType(user, id);
                return PartialView("_CreateByTypeOfFlowView", items);
            }
            catch (ServiceException e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(ReportController)} в методе {nameof(CreateByTypeOfFlowView)}",
                    e);
            }
        }
        
        [UserHasCategoriesAttribute]
        public ActionResult GetTypeOfFlowReport(TempReportModel model, WebUser user, int page = 1)
        {
            try
            {
                var reportModel = _reportModelCreator.CreateByTypeReportModel(model, user, page);
                ViewBag.PageCreator = _pageCreator;
                return PartialView("_GetByTypeOfFlowReport", reportModel);
            }
            catch (WebUiException e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(ReportController)} в методе {nameof(GetTypeOfFlowReport)}",
                    e);
            }
        }
        
        public ActionResult CreateByDatesView()
        {
            return PartialView("_CreateByDatesView");
        }

        [UserHasCategoriesAttribute]
        public ActionResult GetByDatesReport(WebUser user, DateTime dtFrom, DateTime dtTo, int page = 1)
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

        [UserHasCategoriesAttribute]
        public ActionResult GetAllCategoriesReport(WebUser user, DateTime dateFrom, DateTime dateTo,
            int typeOfFlowId)
        {
            ViewBag.TypeOfFlowName = typeOfFlowId == 1 ? "Доход" : "Расход";
            ViewBag.dtFrom = dateFrom;
            ViewBag.dtTo = dateTo;
            try
            {
                var list = _reportControllerHelper.GetOverallList(user, dateFrom, dateTo, typeOfFlowId)
                    .OrderByDescending(x => x.Summ)
                    .ToList();
                ViewBag.Summ = list.Sum(x => x.Summ);
                return PartialView("_GetAllCategoriesReport", list);
            }
            catch (WebUiHelperException e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(ReportController)} в методе {nameof(GetAllCategoriesReport)}",
                    e);
            }
        }

        public PartialViewResult OverallLastYearMonths(WebUser user)
        {
            var model = new ReportMonthsModel();
            try
            {
                var tempQuery = _reportControllerHelper.GetPayingItemsForLastYear(user).ToList();
                _reportControllerHelper.FillReportMonthsModel(model, tempQuery);
                return PartialView("_OverallLastYearMonths", model);
            }
            catch (WebUiHelperException e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(ReportController)} в методе {nameof(OverallLastYearMonths)}",
                    e);
            }
        }

        public ActionResult GetItemsByMonth(WebUser user, DateTime date)
        {
            var dtFrom = date;
            var dtTo = EndDateFromDate(date);
            return RedirectToAction("GetByDatesReport", new {dtFrom, dtTo});
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

        private async Task<List<PayItemSubcategories>> GetPayitemSubcategoriesForView(WebUser user, int typeOfFlowId,
            DateTime date)
        {
            ViewBag.TypeOfFlowName = typeOfFlowId == 1 ? "Доход" : "Расход";
            ViewBag.Month = date.ToString("MMMMMM", CultureInfo.CurrentCulture);
            var dtTo = EndDateFromDate(date);

            try
            {
                var payItemSubcategoriesList =
                    await _payItemSubcategoriesHelper.GetPayItemsWithSubcategoriesInDatesWeb(date, dtTo, user,
                        typeOfFlowId);
                ViewBag.Summ = payItemSubcategoriesList.Sum(x => x.CategorySumm.Summ);
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