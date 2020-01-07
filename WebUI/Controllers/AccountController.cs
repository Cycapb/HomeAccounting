using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.SessionState;
using DomainModels.Model;
using WebUI.Models;
using Services;
using Services.Exceptions;
using WebUI.Exceptions;

namespace WebUI.Controllers
{
    [Authorize]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<ActionResult> Index(WebUser user)
        {
            try
            {
                var list = (await _accountService.GetListAsync())
                    .Where(x => x.UserId == user.Id)
                    .ToList();
                return PartialView(list);
            }
            catch (ServiceException e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(AccountController)} в методе {nameof(Index)}", e);
            }
            catch (Exception e)
            {
                throw  new WebUiException($"Ошибка {e.GetType()} в контроллере {nameof(AccountController)} в методе {nameof(Index)}", e);
            }
        }

        public async Task<ActionResult> Edit(WebUser user, int id)
        {
            try
            {
                var acc = await _accountService.GetItemAsync(id);
                if (acc != null)
                {
                    return PartialView(acc);
                }
                return PartialView(new Account() { UserId = user.Id });
            }
            catch (ServiceException e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(AccountController)} в методе {nameof(Edit)}", e);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Edit(Account account)
        {
            if (account == null)
            {
                return RedirectToAction("Index");
            }

            if (account.AccountID == 0)
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _accountService.UpdateAsync(account);
                    return RedirectToAction("Index");
                }
                catch (ServiceException e)
                {
                    throw new WebUiException($"Ошибка в контроллере {nameof(AccountController)} в методе {nameof(Edit)}", e);
                }
            }

            return PartialView(account);
        }

        public ActionResult Add()
        {
            return PartialView(new AccountAddViewModel());
        }

        [HttpPost]
        public async Task<ActionResult> Add(WebUser user, AccountAddViewModel model)
        {
            if (ModelState.IsValid)
            {
                var account = new Account()
                {
                    AccountName = model.AccountName,
                    Cash = model.Cash,
                    Use = model.Use,
                    UserId = user.Id
                };

                try
                {
                    await _accountService.CreateAsync(account);
                    return RedirectToAction("Index");
                }
                catch (ServiceException e)
                {
                    throw new WebUiException($"Ошибка в контроллере {nameof(AccountController)} в методе {nameof(Add)}", e);
                }
            }
            return PartialView(model);
        }

        public async Task<ActionResult> Delete(WebUser user,int id)
        {
            try
            {
                if (!_accountService.HasAnyDependencies(id))
                {
                    await _accountService.DeleteAsync(id);
                }
            }
            catch (ServiceException e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(AccountController)} в методе {nameof(Delete)}", e);
            }
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> TransferMoney(WebUser user)
        {
            var transfer = new TransferModel();
            try
            {
                await FillTransferModel(user, transfer);
            }
            catch (Exception e)
            {
                throw new WebUiException(
                    $"Ошибка {e.GetType()} в контроллере {nameof(AccountController)} в методе {nameof(TransferMoney)}", e);
            }
            return PartialView(transfer);
        }

        [HttpPost]
        public async Task<ActionResult> TransferMoney(WebUser user, TransferModel tModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var accFrom = await _accountService.GetItemAsync(tModel.FromId);
                    if (!_accountService.HasEnoughMoney(accFrom, decimal.Parse(tModel.Summ)))
                    {
                        ModelState.AddModelError("", "Недостаточно средств на исходном счету");
                        await FillTransferModel(user, tModel);
                        return PartialView(tModel);
                    }
                    var accTo = await _accountService.GetItemAsync(tModel.ToId);
                    await TransferMoney(accFrom, accTo, tModel.Summ);
                    return RedirectToAction("TransferMoney");
                }
                await FillTransferModel(user, tModel);
                return PartialView(tModel);
            }
            catch (ServiceException e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(AccountController)} в методе {nameof(TransferMoney)}", e);
            }
            catch (Exception e)
            {
                throw new WebUiException($"Ошибка {e.GetType()} в контроллере {nameof(AccountController)} в методе {nameof(TransferMoney)}", e);
            }
        }

        public async Task<PartialViewResult> GetItems(int id, WebUser user)
        {
            try
            {
                var list = (await _accountService.GetListAsync())
                .Where(x => x.AccountID != id && x.UserId == user.Id)
                .ToList();
                return PartialView(list);
            }
            catch (ServiceException e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(AccountController)} в методе {nameof(GetItems)}", e);
            }
            catch (Exception e)
            {
                throw new WebUiException($"Ошибка {e.GetType()} в контроллере {nameof(AccountController)} в методе {nameof(GetItems)}", e);
            }
        }

        protected override void Dispose(bool disposing)
        {
            _accountService.Dispose();
            base.Dispose(disposing);
        }

        private async Task FillTransferModel(WebUser user, TransferModel tModel)
        {
            tModel.FromAccounts = (await _accountService.GetListAsync())
                .Where(x => x.UserId == user.Id)
                .ToList();
            tModel.ToAccounts = new List<Account>(tModel.FromAccounts);
        }

        private async Task TransferMoney(Account fromAcc, Account toAcc, string summ)
        {
            fromAcc.Cash -= decimal.Parse(summ);
            toAcc.Cash += decimal.Parse(summ);
            await _accountService.UpdateAsync(fromAcc);
            await _accountService.UpdateAsync(toAcc);
        }
    }
} 