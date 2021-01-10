using DomainModels.Model;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Core.Abstract;
using WebUI.Core.Exceptions;
using WebUI.Core.Infrastructure.Extensions;
using WebUI.Core.Models;
using WebUI.Core.Models.BudgetModels;
using WebUI.Core.Models.Enums;

namespace WebUI.Core.Components
{
    public class MenuIncome : ViewComponent, IDisposable
    {
        private readonly IPayingItemService _payingItemService;
        private readonly IReportHelper _reportHelper;
        private bool _disposed = false;

        public MenuIncome(IPayingItemService payingItemService, IReportHelper reportHelper)
        {
            _payingItemService = payingItemService;
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
                var user = await ViewComponentContext.ViewContext.HttpContext.Session.GetJsonAsync<WebUser>(nameof(WebUser));
                var collection = new List<PayingItem>();

                if (user != null)
                {
                    collection = _payingItemService.GetListByTypeOfFlow(user, (int)TypesOfFlow.Income).ToList();
                }

                var budgetModel = BudgetModel(collection, TypesOfFlow.Income);

                return View("/Views/NavRight/_MenuIncoming.cshtml", budgetModel);
            }
            catch (ServiceException e)
            {
                throw new WebUiException($"Ошибка в ViewComponent с названием {nameof(MenuIncome)} в методе {nameof(InvokeAsync)}", e);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _payingItemService.Dispose();
                    _reportHelper.Dispose();
                    _disposed = true;
                }
            }
        }

        private BudgetModel BudgetModel(List<PayingItem> collection, TypesOfFlow typeOfFlow)
        {
            var budget = new BudgetModel()
            {
                TypeOfFlow = typeOfFlow == TypesOfFlow.Income ? "Доход" : "Расход",
                Month = _reportHelper.GetSummForMonth(collection),
                Week = _reportHelper.GetSummForWeek(collection),
                Day = _reportHelper.GetSummForDay(collection)
            };

            return budget;
        }
    }
}
