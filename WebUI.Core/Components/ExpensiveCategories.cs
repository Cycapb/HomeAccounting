using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Core.Exceptions;
using WebUI.Core.Infrastructure.Extensions;
using WebUI.Core.Models;
using WebUI.Core.Models.CategoryModels;

namespace WebUI.Core.Components
{
    public class ExpensiveCategories : ViewComponent
    {
        private readonly int _itemsPerPage = 10;
        private readonly IPayingItemService _payingItemService;

        public ExpensiveCategories(IPayingItemService payingItemService)
        {
            _payingItemService = payingItemService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                var user = await ViewComponentContext.ViewContext.HttpContext.Session.GetJsonAsync<WebUser>(nameof(WebUser));
                var expensiveCategories = new List<CategorySumModel>();

                if (user != null)
                {
                    expensiveCategories = (await _payingItemService.GetListAsync(x => x.UserId == user.Id && x.Category.TypeOfFlowID == 2 &&
                                x.Date.Month == DateTime.Today.Month && x.Date.Year == DateTime.Today.Year))
                    .GroupBy(x => x.Category.Name)
                    .Select(x => new CategorySumModel() { Category = x.Key, Sum = x.Sum(item => item.Summ) })
                    .OrderByDescending(x => x.Sum)
                    .Take(_itemsPerPage)
                    .ToList();
                }

                return View("/Views/PayingItem/_ExpensiveCategories.cshtml", expensiveCategories);
            }
            catch (ServiceException e)
            {
                throw new WebUiException(
                    $"Ошибка в ViewComponent с названием ExpensiveCategories {nameof(ExpensiveCategories)} в методе {nameof(InvokeAsync)}", e);
            }
        }
    }
}
