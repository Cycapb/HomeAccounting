using DomainModels.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Exceptions;
using System.Collections.Generic;
using System.Linq;
using WebUI.Core.Abstract;
using WebUI.Core.Exceptions;
using WebUI.Core.Models;
using WebUI.Core.Models.BudgetModels;
using WebUI.Core.Models.Enums;

namespace WebUI.Core.Controllers
{
    [Authorize]
    public class NavRightController : Controller
    {
        private readonly IPayingItemService _payingItemService;
        private readonly IReportHelper _reportHelper;

        public NavRightController(IPayingItemService service, IReportHelper reportHelper)
        {
            _payingItemService = service;
            _reportHelper = reportHelper;
        }

        public ActionResult MenuIncoming(WebUser user)
        {
            try
            {
                var collection = _payingItemService.GetListByTypeOfFlow(user, (int)TypesOfFlow.Income)
                    .ToList();
                var budgetModel = BudgetModel(collection, TypesOfFlow.Income);

                return PartialView(budgetModel);
            }
            catch (ServiceException e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(NavRightController)} в методе {nameof(MenuIncoming)}", e);
            }
        }

        public PartialViewResult MenuOutgo(WebUser user)
        {
            try
            {
                var collection = _payingItemService.GetListByTypeOfFlow(user, 2)
                    .ToList();
                var budgetModel = BudgetModel(collection, TypesOfFlow.Outgo);

                return PartialView(budgetModel);
            }
            catch (ServiceException e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(NavRightController)} в методе {nameof(MenuOutgo)}", e);
            }
        }

        protected override void Dispose(bool disposing)
        {
            _reportHelper.Dispose();
            _payingItemService.Dispose();

            base.Dispose(disposing);
        }

        private BudgetModel BudgetModel(List<PayingItem> collection, TypesOfFlow typeOfFlow)
        {
            var budget = new BudgetModel()
            {
                TypeOfMoney = typeOfFlow == TypesOfFlow.Income ? "Доход" : "Расход",
                Month = _reportHelper.GetSummForMonth(collection),
                Week = _reportHelper.GetSummForWeek(collection),
                Day = _reportHelper.GetSummForDay(collection)
            };

            return budget;
        }
    }
}