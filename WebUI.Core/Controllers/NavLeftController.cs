using DomainModels.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Exceptions;
using System.Linq;
using WebUI.Core.Abstract;
using WebUI.Core.Exceptions;
using WebUI.Core.Models;
using WebUI.Core.Models.BudgetModels;

namespace WebUI.Core.Controllers
{
    [Authorize]
    public class NavLeftController : Controller
    {
        private readonly IAccountService _accService;
        private readonly IReportHelper _dbHelper;

        public NavLeftController(IAccountService accService, IReportHelper dbHelper)
        {
            _accService = accService;
            _dbHelper = dbHelper;
        }

        public IActionResult GetAccounts(WebUser user)
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

        public IActionResult GetBudgets(WebUser user)
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
