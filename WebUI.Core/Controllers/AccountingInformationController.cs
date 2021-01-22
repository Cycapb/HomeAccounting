using DomainModels.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Core.Abstract.Helpers;
using WebUI.Core.Exceptions;
using WebUI.Core.Models;
using WebUI.Core.Models.BudgetModels;

namespace WebUI.Controllers
{
    [Authorize]
    public class AccountingInformationController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IReportHelper _reportHelper;
        private readonly IPayingItemService _payingItemService;

        public AccountingInformationController(
            IAccountService accountService,
            IReportHelper reportHelper,
            IPayingItemService payingItemService)
        {
            _accountService = accountService;
            _reportHelper = reportHelper;
            _payingItemService = payingItemService;
        }

        public async Task<IActionResult> Accounts(WebUser user)
        {
            try
            {
                var accounts = await _accountService.GetListAsync(u => u.UserId == user.Id);

                return PartialView("_Accounts", accounts);
            }
            catch (ServiceException e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(AccountingInformationController)} в методе {nameof(Accounts)}", e);
            }
        }

        public async Task<ActionResult> Budgets(WebUser user)
        {
            try
            {
                var budgetViewModel = new OverViewBudgetViewModel
                {
                    BudgetInFact = await _reportHelper.GetBudgetInFactAsync(user),
                    BudgetOverAll = await _reportHelper.GetBudgetOverAllAsync(user)
                };

                return PartialView("_Budgets", budgetViewModel);
            }
            catch (ServiceException e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(AccountingInformationController)} в методе {nameof(Budgets)}", e);
            }
        }

        public async Task<IActionResult> Incomes(WebUser user)
        {
            try
            {
                var collection = (await _payingItemService.GetListByTypeOfFlowAsync(user.Id, 1)).ToList();
                var budgetModel = CreateBudgetModel(collection, 1);

                return PartialView("_IncomesOutgoes", budgetModel);
            }
            catch (ServiceException e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(AccountingInformationController)} в методе {nameof(Incomes)}", e);
            }
        }

        public async Task<IActionResult> Outgoes(WebUser user)
        {
            try
            {
                var collection = (await _payingItemService.GetListByTypeOfFlowAsync(user.Id, 2)).ToList();
                var budgetModel = CreateBudgetModel(collection, 2);

                return PartialView("_IncomesOutgoes", budgetModel);
            }
            catch (ServiceException e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(AccountingInformationController)} в методе {nameof(Outgoes)}", e);
            }
        }

        protected override void Dispose(bool disposing)
        {
            _accountService.Dispose();
            _reportHelper.Dispose();
            _payingItemService.Dispose();

            base.Dispose(disposing);
        }

        private BudgetModel CreateBudgetModel(List<PayingItem> collection, int typeOfFlow)
        {
            var budget = new BudgetModel()
            {
                TypeOfFlow = typeOfFlow == 1 ? "Доход" : "Расход",
                Month = _reportHelper.GetSummForMonth(collection),
                Week = _reportHelper.GetSummForWeek(collection),
                Day = _reportHelper.GetSummForDay(collection)
            };

            return budget;
        }
    }
}