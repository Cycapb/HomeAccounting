using DomainModels.Model;
using Microsoft.AspNetCore.Mvc;
using Services.Exceptions;
using System;
using System.Threading.Tasks;
using WebUI.Core.Abstract.Helpers;
using WebUI.Core.Exceptions;
using WebUI.Core.Infrastructure.Extensions;
using WebUI.Core.Models;
using WebUI.Core.Models.BudgetModels;

namespace WebUI.Core.Components
{
    public class AccountingInformationBudgets : ViewComponent, IDisposable
    {
        private readonly IReportHelper _reportHelper;
        private bool _disposed = false;

        public AccountingInformationBudgets(IReportHelper reportHelper)
        {
            _reportHelper = reportHelper;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                var user = await ViewContext.HttpContext.Session.GetJsonAsync<WebUser>(nameof(WebUser));

                var budgetViewModel = new OverViewBudgetViewModel()
                {
                    BudgetInFact = 0M.ToString("c"),
                    BudgetOverAll = 0M.ToString("c"),
                };

                if (user != null)
                {
                    budgetViewModel = await FillBudgetViewModel(user.Id);
                }

                return View("/Views/AccountingInformation/_Budgets.cshtml", budgetViewModel);
            }
            catch (ServiceException e)
            {
                throw new WebUiException($"Ошибка в ViewComponent с названием {nameof(AccountingInformationBudgets)} в методе {nameof(InvokeAsync)}", e);
            }
            catch (Exception ex)
            {
                throw new WebUiException($"Ошибка в ViewComponent с названием {nameof(AccountingInformationBudgets)} в методе {nameof(InvokeAsync)}", ex);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _reportHelper.Dispose();
                    _disposed = true;
                }
            }
        }

        private async Task<OverViewBudgetViewModel> FillBudgetViewModel(string userId)
        {
            try
            {
                var budget = new OverViewBudgetViewModel
                {
                    BudgetInFact = await _reportHelper.GetBudgetInFactAsync(userId),
                    BudgetOverAll = await _reportHelper.GetBudgetOverAllAsync(userId)
                };

                return budget;
            }
            catch (ServiceException e)
            {
                throw new WebUiException($"Ошибка в ViewComponent с названием {nameof(AccountingInformationBudgets)} в методе {nameof(FillBudgetViewModel)}", e);
            }
        }
    }
}
