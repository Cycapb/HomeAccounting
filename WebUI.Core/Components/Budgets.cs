using DomainModels.Model;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Exceptions;
using System;
using System.Threading.Tasks;
using WebUI.Core.Abstract;
using WebUI.Core.Exceptions;
using WebUI.Core.Infrastructure.Extensions;
using WebUI.Core.Models;
using WebUI.Core.Models.BudgetModels;

namespace WebUI.Core.Components
{
    public class Budgets : ViewComponent, IDisposable
    {
        private readonly IAccountService _accountService;
        private readonly IReportHelper _reportHelper;
        private bool _disposed = false;

        public Budgets(IAccountService accountService, IReportHelper reportHelper)
        {
            _accountService = accountService;
            _reportHelper = reportHelper;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await ViewContext.HttpContext.Session.GetJsonAsync<WebUser>("WebUser");
            var budgetViewModel = await GetBudget(user);

            return View("Views/NavLeft/_Budgets.cshtml", budgetViewModel);
        }

        private async Task<OverViewBudgetViewModel> GetBudget(IWorkingUser user)
        {
            try
            {
                var budget = new OverViewBudgetViewModel
                {
                    BudgetInFact = await _reportHelper.GetBudgetInFactWeb(user),
                    BudgetOverAll = await _reportHelper.GetBudgetOverAllWeb(user)
                };

                return budget;
            }
            catch (ServiceException e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(Budgets)} в методе {nameof(GetBudget)}", e);
            }
        }
    }
}
