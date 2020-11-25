using DomainModels.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Core.Exceptions;
using WebUI.Core.Models;

namespace WebUI.Core.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<IActionResult> Index(WebUser user)
        {
            try
            {
                var list = (await _accountService.GetListAsync(x => x.UserId == user.Id)).ToList();

                return PartialView("_Index", list);
            }
            catch (ServiceException e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(AccountController)} в методе {nameof(Index)}", e);
            }
            catch (Exception e)
            {
                throw new WebUiException($"Ошибка {e.GetType()} в контроллере {nameof(AccountController)} в методе {nameof(Index)}", e);
            }
        }

        public async Task<IActionResult> Edit(WebUser user, int id)
        {
            try
            {
                var acc = await _accountService.GetItemAsync(id);

                if (acc != null)
                {
                    return PartialView("_Edit", acc);
                }

                return RedirectToAction("Index");
            }
            catch (ServiceException e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(AccountController)} в методе {nameof(Edit)}", e);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Account account)
        {
            if (account is null)
            {
                throw new ArgumentNullException(nameof(account));
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

            return PartialView("_Edit", account);
        }

        public IActionResult Add()
        {
            return PartialView("_Add", new AccountAddModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(WebUser user, AccountAddModel model)
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

            return PartialView("_Add", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
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

        public async Task<IActionResult> TransferMoney(WebUser user)
        {
            try
            {
                var transfer = new TransferModel();
                await FillTransferModel(user, transfer);

                return PartialView("_TransferMoney", transfer);
            }
            catch (Exception e)
            {
                throw new WebUiException(
                    $"Ошибка {e.GetType()} в контроллере {nameof(AccountController)} в методе {nameof(TransferMoney)}", e);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //ToDo Вынести логику перевода денег из контроллера в сервис IAccountService
        public async Task<IActionResult> TransferMoney(WebUser user, TransferModel tModel)
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

                        return PartialView("_TransferMoney", tModel);
                    }

                    var accTo = await _accountService.GetItemAsync(tModel.ToId);
                    await TransferMoney(accFrom, accTo, tModel.Summ);

                    return RedirectToAction("TransferMoney");
                }

                await FillTransferModel(user, tModel);

                return PartialView("TransferMoney", tModel);
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

        public async Task<PartialViewResult> GetUserAccounts(int id, WebUser user)
        {
            try
            {
                var accounts = (await _accountService.GetListAsync(x => x.AccountID != id && x.UserId == user.Id)).ToList();

                return PartialView(accounts);
            }
            catch (ServiceException e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(AccountController)} в методе {nameof(GetUserAccounts)}", e);
            }
            catch (Exception e)
            {
                throw new WebUiException($"Ошибка {e.GetType()} в контроллере {nameof(AccountController)} в методе {nameof(GetUserAccounts)}", e);
            }
        }

        protected override void Dispose(bool disposing)
        {
            _accountService.Dispose();
            base.Dispose(disposing);
        }

        private async Task FillTransferModel(WebUser user, TransferModel tModel)
        {
            tModel.FromAccounts = (await _accountService.GetListAsync(x => x.UserId == user.Id)).ToList();
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