using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.SessionState;
using DomainModels.Model;
using WebUI.Models;
using Services;

namespace WebUI.Controllers
{
    [Authorize]
    [SessionState(SessionStateBehavior.ReadOnly )]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<ActionResult> Index(WebUser user)
        {
            var list = (await _accountService.GetListAsync())
                .Where(x => x.UserId == user.Id)
                .ToList();
            return PartialView(list);
        }

        public async Task<ActionResult> Edit(WebUser user, int id)
        {
            var acc = await _accountService.GetItemAsync(id);
            if (acc != null)
            {
                return PartialView(acc);
            }
            return PartialView(new Account() {UserId = user.Id});
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
                await _accountService.UpdateAsync(account);
                return RedirectToAction("Index");
            }

            return PartialView(account);
        }

        public ActionResult Add(WebUser user)
        {
            return PartialView(new Account() {UserId = user.Id});
        }

        [HttpPost]
        public async Task<ActionResult> Add(WebUser user, Account account)
        {
            if (ModelState.IsValid)
            {
                await _accountService.Create(account);
                return RedirectToAction("Index");
            }
            return PartialView(account);
        }

        public async Task<ActionResult> Delete(WebUser user,int id)
        {
            if (!_accountService.HasAnyDependencies(id))
            {
                await _accountService.DeleteAsync(id);
            }
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> TransferMoney(WebUser user)
        {
            var transfer = new TransferModel();
            await FillTransferModel(user, transfer);
            return PartialView(transfer);
        }

        [HttpPost]
        public async Task<ActionResult> TransferMoney(WebUser user, TransferModel tModel)
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

        public async Task<PartialViewResult> GetItems(int id, WebUser user)
        {
            var list = (await _accountService.GetListAsync())
                .Where(x => x.AccountID != id && x.UserId == user.Id)
                .ToList();
            return PartialView(list);
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