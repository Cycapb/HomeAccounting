using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.SessionState;
using HomeAccountingSystem_DAL.Model;
using HomeAccountingSystem_WebUI.Models;
using Services;

namespace HomeAccountingSystem_WebUI.Controllers
{
    [Authorize]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class DebtController : Controller
    {
        private readonly IDebtService _debtManager;
        private readonly IAccountService _accService;


        public DebtController(IDebtService debtManager, IAccountService accService)
        {
            _debtManager = debtManager;
            _accService = accService;
        }

        public PartialViewResult Index(WebUser user)
        {
            var items = _debtManager.GetOpenUserDebts(user.Id).ToList();
            var model = new DebtsSumModel()
            {
                MyDebtsSumm = items.Where(x => x.TypeOfFlowId == 1).Sum(x => x.Summ),
                DebtsToMeSumm = items.Where(x => x.TypeOfFlowId == 2).Sum(x => x.Summ)
            };
            return PartialView("IndexPartial",model);
        }

        public PartialViewResult DebtList(WebUser user)
        {
            var items = _debtManager.GetOpenUserDebts(user.Id).ToList();
            var model = new DebtsModel()
            {
                MyDebts = items.Where(x => x.TypeOfFlowId == 1).ToList(),
                DebtsToMe = items.Where(x => x.TypeOfFlowId == 2).ToList()
            };
            return PartialView("DebtList",model);
        }

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
                await _debtManager.CreateAsync(debt);
                return RedirectToAction("DebtList");
            }
            model.Accounts = (await AccountList(user.Id)).ToList();
            return PartialView(model);
        }

        [HttpPost]
        public async Task<ActionResult> Close(int id)
        {
            await _debtManager.CloseAsync(id);
            return RedirectToAction("DebtList");
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            await _debtManager.DeleteAsync(id);
            return RedirectToAction("DebtList");
        }
        private async Task<IEnumerable<Account>> AccountList(string userId)
        {
            return (await _accService.GetListAsync()).Where(x => x.UserId == userId).ToList();
        }

    }
}