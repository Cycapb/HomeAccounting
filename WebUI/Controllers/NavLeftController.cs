using DomainModels.Model;
using Services;
using Services.Exceptions;
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
    public class NavLeftController : Controller
    {
        private readonly IAccountService _accService;
        private readonly IReportHelper _dbHelper;

        public NavLeftController(IAccountService accService, IReportHelper dbHelper)
        {
            _accService = accService;
            _dbHelper = dbHelper;
        }

        public ActionResult GetAccounts(WebUser user)
        {
            try
            {
                var accounts = _accService.GetList(u => u.UserId == user.Id)
                    .ToList();

                return PartialView(accounts);
            }
            catch (ServiceException e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(NavLeftController)} в методе {nameof(GetAccounts)}", e);
            }
        }

        public ActionResult GetBudgets(WebUser user)
        {
            return PartialView(GetBudget(user));
        }

        protected override void Dispose(bool disposing)
        {
            _accService.Dispose();
            _dbHelper.Dispose();

            base.Dispose(disposing);
        }

        private OverViewBudgetViewModel GetBudget(IWorkingUser user)
        {
            try
            {
                var budget = new OverViewBudgetViewModel
                {
                    BudgetInFact = _dbHelper.GetBudgetInFactWeb(user).Result,
                    BudgetOverAll = _dbHelper.GetBudgetOverAllWeb(user).Result
                };
                return budget;
            }
            catch (ServiceException e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(NavLeftController)} в методе {nameof(GetBudget)}", e);
            }
        }
    }
}