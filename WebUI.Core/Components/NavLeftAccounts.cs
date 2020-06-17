using DomainModels.Model;
using Microsoft.AspNetCore.Http;
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

namespace WebUI.Core.Components
{
    public class NavLeftAccounts : ViewComponent
    {
        private readonly IAccountService _accountService;

        public NavLeftAccounts(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                var accounts = new List<Account>();
                var user = await HttpContext.Session.GetJsonAsync<WebUser>("WebUser");
                
                if (user != null)
                {
                    var taskResult = await _accountService.GetListAsync(u => u.UserId == user.Id);
                    accounts = taskResult.ToList();
                }

                return View("/Views/NavLeft/_Accounts.cshtml", accounts);
            }
            catch (ServiceException e)
            {
                throw new WebUiException($"Ошибка в компоненте {nameof(NavLeftAccounts)} в методе {nameof(InvokeAsync)}", e);
            }
            catch (Exception ex)
            {
                throw new WebUiException($"Ошибка в компоненте {nameof(NavLeftAccounts)} в методе {nameof(InvokeAsync)}", ex);
            }
        }
    }
}
