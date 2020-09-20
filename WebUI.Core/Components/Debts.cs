using Microsoft.AspNetCore.Mvc;
using Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Core.Exceptions;
using WebUI.Core.Infrastructure.Extensions;
using WebUI.Core.Models;
using WebUI.Core.Models.DebtModels;

namespace WebUI.Core.Components
{
    public class Debts : ViewComponent
    {
        private readonly IDebtService _debtService;

        public Debts(IDebtService debtService)
        {
            _debtService = debtService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                var user = await ViewContext.HttpContext.Session.GetJsonAsync<WebUser>(nameof(WebUser));
                var model = new DebtsSumModel()
                {
                    DebtsToMeSumm = 0M,
                    MyDebtsSumm = 0M
                };

                if (user != null)
                {
                    var items = _debtService.GetOpenUserDebts(user.Id).ToList();
                    model = new DebtsSumModel()
                    {
                        MyDebtsSumm = items.Where(x => x.TypeOfFlowId == 1).Sum(x => x.Summ),
                        DebtsToMeSumm = items.Where(x => x.TypeOfFlowId == 2).Sum(x => x.Summ)
                    };
                }

                return View("/Views/Debt/_Index.cshtml", model);
            }
            catch (Exception e)
            {
                throw new WebUiException($"Ошибка {e.GetType()} в ViewComponent с названием {nameof(Debts)} в методе {nameof(InvokeAsync)}", e);
            }
        }
    }
}
