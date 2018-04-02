using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.SessionState;
using DomainModels.Model;
using WebUI.Models;
using WebUI.Infrastructure.Attributes;
using Services;
using Services.Exceptions;
using WebUI.Exceptions;

namespace WebUI.Controllers
{
    [Authorize]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class DebtController : Controller
    {
        private readonly IDebtService _debtService;
        private readonly ICreateCloseDebtService _createCloseDebtService;
        private readonly IAccountService _accService;


        public DebtController(IDebtService debtService, 
            ICreateCloseDebtService createCloseDebtService, 
            IAccountService accService)
        {
            _debtService = debtService;
            _createCloseDebtService = createCloseDebtService;
            _accService = accService;
        }

        public PartialViewResult Index(WebUser user)
        {
            DebtsSumModel model;
            try
            {
                var items = _debtService.GetOpenUserDebts(user.Id).ToList();
                model = new DebtsSumModel()
                {
                    MyDebtsSumm = items.Where(x => x.TypeOfFlowId == 1).Sum(x => x.Summ),
                    DebtsToMeSumm = items.Where(x => x.TypeOfFlowId == 2).Sum(x => x.Summ)
                };
            }
            catch (Exception e)
            {
                throw new WebUiException($"Ошибка {e.GetType()} в контроллере {nameof(DebtController)} в методе {nameof(Index)}", e);
            }

            return PartialView("IndexPartial",model);
        }

        public PartialViewResult DebtList(WebUser user)
        {
            DebtsModel model;
            try
            {
                var items = _debtService.GetOpenUserDebts(user.Id).ToList();
                model = new DebtsModel()
                {
                    MyDebts = items.Where(x => x.TypeOfFlowId == 1).ToList(),
                    DebtsToMe = items.Where(x => x.TypeOfFlowId == 2).ToList()
                };
            }
            catch (Exception e)
            {
                throw new WebUiException($"Ошибка {e.GetType()} в контроллере {nameof(DebtController)} в методе {nameof(DebtList)}", e);
            }
            
            return PartialView("DebtList",model);
        }

        [UserHasAnyAccount]
        public async Task<ActionResult> Add(WebUser user)
        {
            var model = new DebtsAddModel()
            {
                Accounts = (await AccountList(user.Id)).ToList()
            };
            return PartialView(model);
        }

        [HttpPost]
        public async Task<ActionResult> Add(WebUser user, DebtsAddModel model)
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
                }
                catch (ServiceException e)
                {
                    throw new WebUiException($"Ошибка в контроллере {nameof(DebtController)} в методе {nameof(Add)}", e);
                }
                
                return RedirectToAction("DebtList");
            }
            model.Accounts = (await AccountList(user.Id)).ToList();
            return PartialView(model);
        }

        [HttpPost]
        public async Task<ActionResult> Close(int id)
        {
            try
            {
                await _createCloseDebtService.CloseAsync(id);
            }
            catch (ServiceException e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(DebtController)} в методе {nameof(Close)}", e);
            }
            
            return RedirectToAction("DebtList");
        }

        public async Task<ActionResult> ClosePartially(int id)
        {
            try
            {
                var debt = await _debtService.GetItemAsync(id);
                if (debt == null)
                {
                    return RedirectToAction("DebtList");
                }
                var debtEditModel = new DebtEditViewModel()
                {
                    DebtId = debt.DebtID,
                    Comment = debt.Person,
                    Sum = debt.Summ
                };
                return PartialView("_ClosePartially", debtEditModel);
            }
            catch (ServiceException e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(DebtController)} в методе {nameof(ClosePartially)}", e);
            }
        }

        [HttpPost]
        public async Task<ActionResult> ClosePartially(DebtEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _createCloseDebtService.PartialCloseAsync(model.DebtId, model.Sum);
                }
                catch (ServiceException e)
                {
                    throw new WebUiException(
                        $"Ошибка в контроллере {nameof(DebtController)} в методе {nameof(ClosePartially)}", e);
                }
                catch (ArgumentOutOfRangeException)
                {
                    TempData["message"] = "Введенная сумма больше суммы долга";
                    return PartialView("_ClosePartially", model);
                }
                return RedirectToAction("DebtList");
            }

            return PartialView("_ClosePartially", model);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _debtService.DeleteAsync(id);
            }
            catch (ServiceException e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(DebtController)} в методе {nameof(Delete)}", e);
            }
            
            return RedirectToAction("DebtList");
        }
        private async Task<IEnumerable<Account>> AccountList(string userId)
        {
            try
            {
                return (await _accService.GetListAsync()).Where(x => x.UserId == userId).ToList();
            }
            catch (ServiceException e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(DebtController)} в методе {nameof(AccountList)}", e);
            }
            catch (Exception e)
            {
                throw new WebUiException($"Ошибка {e.GetType()} в контроллере {nameof(DebtController)} в методе {nameof(AccountList)}", e);
            }
        }

    }
}