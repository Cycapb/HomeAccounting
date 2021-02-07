using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using WebUI.Core.Exceptions;
using WebUI.Core.Models;
using WebUI.Core.Models.DebtModels;
using WebUI.Core.Infrastructure.Filters;
using DomainModels.Model;
using Services.Exceptions;

namespace WebUI.Core.Controllers
{
    [Authorize]
    public class DebtController : Controller
    {
        private readonly IDebtService _debtService;
        private readonly ICreateCloseDebtService _createCloseDebtService;
        private readonly IAccountService _accService;

        public DebtController(
            IDebtService debtService,
            ICreateCloseDebtService createCloseDebtService,
            IAccountService accService)
        {
            _debtService = debtService;
            _createCloseDebtService = createCloseDebtService;
            _accService = accService;
        }

        public async Task<IActionResult> Index(WebUser user)
        {
            try
            {
                var openUserDebts = (await _debtService.GetOpenUserDebtsAsync(user.Id)).ToList();
                var model = new DebtsSumModel()
                {
                    MyDebtsSumm = openUserDebts.Where(x => x.TypeOfFlowId == 1).Sum(x => x.Summ),
                    DebtsToMeSumm = openUserDebts.Where(x => x.TypeOfFlowId == 2).Sum(x => x.Summ)
                };

                return PartialView("_Index", model);
            }
            catch (Exception e)
            {
                throw new WebUiException($"Ошибка {e.GetType()} в контроллере {nameof(DebtController)} в методе {nameof(Index)}", e);
            }
        }

        public async Task<IActionResult> DebtList(WebUser user)
        {            
            try
            {
                var items = (await _debtService.GetOpenUserDebtsAsync(user.Id)).ToList();
                var model = new DebtsCollectionModel()
                {
                    MyDebts = items.Where(x => x.TypeOfFlowId == 1).ToList(),
                    DebtsToMe = items.Where(x => x.TypeOfFlowId == 2).ToList()
                };

                return PartialView("_Debts", model);
            }
            catch (Exception e)
            {
                throw new WebUiException($"Ошибка {e.GetType()} в контроллере {nameof(DebtController)} в методе {nameof(DebtList)}", e);
            }
        }

        [TypeFilter(typeof(UserHasAnyAccount))]
        public async Task<IActionResult> Add(WebUser user)
        {
            var userAccounts = (await GetAccountsByUserId(user.Id)).ToList();
            var model = new DebtAddModel()
            {
                Accounts = userAccounts
            };

            return PartialView("_Add", model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(WebUser user, DebtAddModel model)
        {
            if (ModelState.IsValid)
            {
                var debt = new Debt()
                {
                    DateBegin = DateTime.Now,
                    Person = model.Person,
                    TypeOfFlowId = model.TypeOfFlowId,
                    AccountId = model.AccountId,
                    Summ = model.Summ,
                    UserId = user.Id
                };

                try
                {
                    await _createCloseDebtService.CreateAsync(debt);

                    return RedirectToAction("DebtList");
                }
                catch (ServiceException e)
                {
                    throw new WebUiException($"Ошибка в контроллере {nameof(DebtController)} в методе {nameof(Add)}", e);
                }
            }

            var userAccounts = (await GetAccountsByUserId(user.Id)).ToList();
            model.Accounts = userAccounts;

            return PartialView("_Add", model);
        }

        public async Task<IActionResult> ClosePartially(int id)
        {
            try
            {
                var debt = await _debtService.GetItemAsync(id);

                if (debt == null)
                {
                    return RedirectToAction("DebtList");
                }

                var debtEditModel = new DebtEditModel();
                await FillDebtViewModel(debt, debtEditModel);

                return PartialView("_ClosePartially", debtEditModel);
            }
            catch (ServiceException e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(DebtController)} в методе {nameof(ClosePartially)}", e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ClosePartially(DebtEditModel model)
        {
            Debt debt;

            if (ModelState.IsValid)
            {
                try
                {
                    await _createCloseDebtService.PartialCloseAsync(model.DebtId, model.Sum, model.AccountId);
                }
                catch (ServiceException e)
                {
                    throw new WebUiException(
                        $"Ошибка в контроллере {nameof(DebtController)} в методе {nameof(ClosePartially)}", e);
                }
                catch (ArgumentOutOfRangeException)
                {
                    ModelState.AddModelError("", "Введенная сумма больше суммы долга");
                    debt = await _debtService.GetItemAsync(model.DebtId);
                    await FillDebtViewModel(debt, model);

                    return PartialView("_ClosePartially", model);
                }

                return RedirectToAction("DebtList");
            }

            debt = await _debtService.GetItemAsync(model.DebtId);
            await FillDebtViewModel(debt, model);

            return PartialView("_ClosePartially", model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _debtService.DeleteAsync(id);

                return RedirectToAction("DebtList");
            }
            catch (ServiceException e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(DebtController)} в методе {nameof(Delete)}", e);
            }
        }

        protected override void Dispose(bool disposing)
        {
            _accService.Dispose();
            _debtService.Dispose();
            _createCloseDebtService.Dispose();

            base.Dispose(disposing);
        }

        private async Task<IEnumerable<Account>> GetAccountsByUserId(string userId)
        {
            try
            {
                return (await _accService.GetListAsync(x => x.UserId == userId)).ToList();
            }
            catch (ServiceException e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(DebtController)} в методе {nameof(GetAccountsByUserId)}", e);
            }
            catch (Exception e)
            {
                throw new WebUiException($"Ошибка {e.GetType()} в контроллере {nameof(DebtController)} в методе {nameof(GetAccountsByUserId)}", e);
            }
        }

        private async Task FillDebtViewModel(Debt debt, DebtEditModel model)
        {
            var accounts = (await _accService.GetListAsync(a => a.UserId == debt.UserId)).ToList();

            model.DebtId = debt.DebtID;
            model.Sum = debt.Summ;
            model.Accounts = accounts;
            model.Date = debt.DateBegin.ToShortDateString();
            model.Person = debt.Person;
            model.TypeOfFlowId = debt.TypeOfFlowId;
            model.AccountId = debt.AccountId;
        }

    }
}