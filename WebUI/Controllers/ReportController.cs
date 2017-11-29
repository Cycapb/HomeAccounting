using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.SessionState;
using DomainModels.Model;
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

        public ReportController(IPayItemSubcategoriesHelper payItemSubcategoriesHelper, 
            IReportControllerHelper reportControllerHelper,
            IReportModelCreator reportModelCreator)
        {
            _payItemSubcategoriesHelper = payItemSubcategoriesHelper;
            _reportControllerHelper = reportControllerHelper;
            _reportModelCreator = reportModelCreator;
        }

        public ViewResult Index()
        {
            return View();
        }

        public ViewResult CreateByTypeOfFlowView(WebUser user, int id)
        {
            ViewBag.TypeOfFlowId = id;
            try
            {
                var items = _reportControllerHelper.GetCategoriesByType(user, id);
                return View(items);
            }
            catch (ServiceException e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(ReportController)} в методе {nameof(CreateByTypeOfFlowView)}",
                    e);
            }
        }

        public ActionResult GetByTypeOfFlowReportPartial(TempReportModel model, WebUser user, int page = 1)
        {
            try
            {
                var reportModel = _reportModelCreator.CreateByTypeReportModel(model, user, page);
                return PartialView(reportModel);
            }
            catch (ServiceException e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(ProductController)} в методе {nameof(GetByTypeOfFlowReportPartial)}",
                    e);
            }
        }
        
        [UserHasAnyCategories]
        public ActionResult GetTypeOfFlowReport(TempReportModel model, WebUser user, int page = 1)
        {
            var reportModel = _reportModelCreator.CreateByTypeReportModel(model, user, page);
            return PartialView(reportModel);
        }
        
        public ActionResult CreateByDatesView()
        {
            return View();
        }

        public ActionResult GetByDatesReportView(WebUser user, DateTime dtFrom, DateTime dtTo, int page = 1)
        {
            FillViewBag(dtFrom, dtTo, user);
            var reportModel = _reportModelCreator.CreateByDatesReportModel(user, dtFrom, dtTo, page);
            return View(reportModel);
        }

        [UserHasAnyCategories]
        public ActionResult GetByDatesReport(WebUser user, DateTime dtFrom, DateTime dtTo, int page = 1)
        {
            FillViewBag(dtFrom, dtTo, user);
            var reportModel = _reportModelCreator.CreateByDatesReportModel(user, dtFrom, dtTo, page);
            return PartialView(reportModel);
        }

        public ActionResult GetByDatesReportPartial(WebUser user, DateTime dtFrom, DateTime dtTo, int page = 1)
        {
            var reportModel = _reportModelCreator.CreateByDatesReportModel(user, dtFrom, dtTo, page);
            return PartialView(reportModel);
        }

        [UserHasAnyCategories]
        public PartialViewResult GetAllCategoriesReport(WebUser user, DateTime dateFrom, DateTime dateTo,
            int typeOfFlowId)
        {
            ViewBag.TypeOfFlowName = typeOfFlowId == 1 ? "Доход" : "Расход";
            ViewBag.dtFrom = dateFrom;
            ViewBag.dtTo = dateTo;
            var list = _reportControllerHelper.GetOverallList(user, dateFrom, dateTo, typeOfFlowId).ToList();
            ViewBag.Summ = list.Sum(x => x.Summ);
            return PartialView(list);
        }

        public PartialViewResult OverallLastYearMonths(WebUser user)
        {
            var model = new ReportMonthsModel();
            var tempQuery = _reportControllerHelper.GetPayingItemsForLastYear(user).ToList();
            _reportControllerHelper.FillReportMonthsModel(model, tempQuery);
            return PartialView(model);
        }

        public ActionResult GetItemsByMonth(WebUser user, DateTime date)
        {
            var dtFrom = date;
            var dtTo = EndDateFromDate(date);
            if (Request.IsAjaxRequest())
            {
                return RedirectToAction("GetByDatesReport", new {dtFrom, dtTo});
            }
            return RedirectToAction("GetByDatesReportView", new {dtFrom, dtTo});
        }

        public async Task<ActionResult> SubcategoriesReportView(WebUser user, int typeOfFlowId, DateTime date)
        {
            var payItemSubcategoriesList = await GetPayitemSubcategoriesForView(user, typeOfFlowId, date);
            return View(payItemSubcategoriesList);
        }

        public async Task<ActionResult> SubcategoriesReport(WebUser user, int typeOfFlowId, DateTime date)
        {
            var payItemSubcategoriesList = await GetPayitemSubcategoriesForView(user, typeOfFlowId, date);
            return PartialView(payItemSubcategoriesList);
        }

        private async Task<List<PayItemSubcategories>> GetPayitemSubcategoriesForView(WebUser user, int typeOfFlowId, DateTime date)
        {
            ViewBag.TypeOfFlowName = typeOfFlowId == 1 ? "Доход" : "Расход";
            ViewBag.Month = date.ToString("MMMMMM", CultureInfo.CurrentCulture);
            var dtTo = EndDateFromDate(date);

            var payItemSubcategoriesList =
                await _payItemSubcategoriesHelper.GetPayItemsWithSubcategoriesInDatesWeb(date, dtTo, user, typeOfFlowId);
            ViewBag.Summ = payItemSubcategoriesList.Sum(x => x.CategorySumm.Summ);
            return payItemSubcategoriesList;
        } 

        private DateTime EndDateFromDate(DateTime date)
        {
            var daysInmonth = DateTime.DaysInMonth(date.Year, date.Month);
            return date + TimeSpan.FromDays(daysInmonth - 1);
        }
        
        private decimal GetSummOfItems(List<PayingItem> list, int typeOfFlowId)
        {
            return list
                .Where(x => x.Category.TypeOfFlowID == typeOfFlowId)
                .Sum(x => x.Summ);
                
        }

        private void FillViewBag(DateTime dtFrom, DateTime dtTo, WebUser user)
        {
            var payingItemList = _reportControllerHelper.GetPayingItemsInDates(dtFrom,dtTo,user).ToList();
            ViewBag.OutgoSum = GetSummOfItems(payingItemList, 2);
            ViewBag.IncomingSum = GetSummOfItems(payingItemList, 1);
        }
    }
}
