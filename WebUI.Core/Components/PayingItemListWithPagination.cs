using DomainModels.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services;
using Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Core.Exceptions;
using WebUI.Core.Infrastructure.Extensions;
using WebUI.Core.Models;
using WebUI.Core.Models.PayingItemModels;

namespace WebUI.Core.Components
{
    public class PayingItemListWithPagination : ViewComponent
    {
        private readonly IPayingItemService _payingItemService;
        private readonly int _itemsToShowOnPage = 10;
        private readonly int _pageNumber = 1;

        public PayingItemListWithPagination(IPayingItemService payingItemService)
        {
            _payingItemService = payingItemService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                var user = await ViewContext.HttpContext.Session.GetJsonAsync<WebUser>(nameof(WebUser));
                var model = new PayingItemsListWithPaginationModel()
                {
                    PagingInfo = new PagingInfo() 
                    {
                        CurrentPage = _pageNumber,
                        ItemsPerPage = _itemsToShowOnPage,
                        TotalItems = 10
                    },
                    PayingItems = new List<PayingItem>()
                };

                if (user != null)
                {
                    var result = await _payingItemService.GetListAsync(i => DateTime.Now.Date - i.Date <= TimeSpan.FromDays(2) && i.UserId == user.Id);
                    var items = result.ToList();
                    model = new PayingItemsListWithPaginationModel()
                    {
                        PayingItems = items
                            .OrderByDescending(i => i.Date)
                            .ThenBy(x => x.Category.Name)
                            .Skip((_pageNumber - 1) * _itemsToShowOnPage)
                            .Take(_itemsToShowOnPage),
                        PagingInfo = new PagingInfo()
                        {
                            CurrentPage = _pageNumber,
                            ItemsPerPage = _itemsToShowOnPage,
                            TotalItems = items.Count
                        }
                    };

                }

                return View("/Views/PayingItem/_List.cshtml", model);
            }
            catch (ServiceException e)
            {
                throw new WebUiException($"Ошибка в ViewComponent с названием {nameof(PayingItemListWithPagination)} в методе {nameof(InvokeAsync)}", e);
            }
            catch (Exception e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(PayingItemListWithPagination)} в методе {nameof(InvokeAsync)}", e);
            }
        }
    }
}
