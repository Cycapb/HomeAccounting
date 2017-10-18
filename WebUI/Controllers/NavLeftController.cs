using System.Linq;
using System.Web.Mvc;
using System.Web.SessionState;
using WebUI.Models;
using Services;

namespace WebUI.Controllers
{
    [Authorize]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class NavLeftController : Controller
    {
        private readonly IAccountService _accService;
        private readonly IDbHelper _dbHelper;

        public NavLeftController(IAccountService accService, IDbHelper dbHelper)
        {
            _accService = accService;
            _dbHelper = dbHelper;
        }

        public ActionResult GetAccounts(WebUser user)
        {
            var accounts = _accService.GetList()
                .Where(u=>u.UserId == user.Id)
                .ToList();
            return PartialView(accounts);
        }

        public ActionResult GetBudgets(WebUser user)
        {
            return PartialView(GetBudget(user));
        }

        private Budget GetBudget(WebUser user)
        {
            var budget = new Budget();

            budget.BudgetInFact = _dbHelper.GetBudgetInFactWeb(user).Result;
            budget.BudgetOverAll = _dbHelper.GetBudgetOverAllWeb(user).Result;

            return budget;
        }
    }
}