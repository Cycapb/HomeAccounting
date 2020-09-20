using DomainModels.Model;
using Services;
using Services.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.SessionState;
using WebUI.Abstract;
using WebUI.Exceptions;
using WebUI.Models;
using WebUI.Models.BudgetModels;

namespace WebUI.Controllers
{
    [Authorize]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class NavRightController : Controller
    {
        private readonly IPayingItemService _payingItemService;
        private readonly IReportHelper _dbHelper;

        public NavRightController(IPayingItemService service, IReportHelper dbHelper)
        {
            _payingItemService = service;
            _dbHelper = dbHelper;
        }

        public ActionResult MenuIncoming(WebUser user)
        {
            List<PayingItem> collection;
            try
            {
                collection = _payingItemService.GetListByTypeOfFlow(user, 1)
                    .ToList();
            }
            catch (ServiceException e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(NavRightController)} в методе {nameof(MenuIncoming)}", e);
            }

            var budgetModel = BudgetModel(collection, 1);
            return PartialView(budgetModel);
        }

        public PartialViewResult MenuOutgo(WebUser user)
        {
            List<PayingItem> collection;
            try
            {
                collection = _payingItemService.GetListByTypeOfFlow(user, 2)
                    .ToList();
            }
            catch (ServiceException e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(NavRightController)} в методе {nameof(MenuOutgo)}", e);
            }

            var budgetModel = BudgetModel(collection, 2);
            return PartialView(budgetModel);
        }

        protected override void Dispose(bool disposing)
        {
            _dbHelper.Dispose();
            _payingItemService.Dispose();

            base.Dispose(disposing);
        }

        private BudgetModel BudgetModel(List<PayingItem> collection, int typeOfMoney)
        {
            var budget = new BudgetModel()
            {
                TypeOfMoney = typeOfMoney == 1 ? "Доход" : "Расход",
                Month = _dbHelper.GetSummForMonth(collection),
                Week = _dbHelper.GetSummForWeek(collection),
                Day = _dbHelper.GetSummForDay(collection)
            };

            return budget;
        }
    }
}