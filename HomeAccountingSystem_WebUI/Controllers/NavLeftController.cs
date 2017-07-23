using System.Linq;
using System.Web.Mvc;
using System.Web.SessionState;
using DomainModels.Model;
using DomainModels.Repositories;
using WebUI.Models;
using Services;

namespace WebUI.Controllers
{
    [Authorize]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class NavLeftController : Controller
    {
        private readonly IRepository<Account> _repository;
        private readonly IDbHelper _dbHelper;

        public NavLeftController(IRepository<Account> repository, IDbHelper dbHelper)
        {
            _repository = repository;
            _dbHelper = dbHelper;
        }

        public ActionResult GetAccounts(WebUser user)
        {
            var accounts = _repository.GetList()
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