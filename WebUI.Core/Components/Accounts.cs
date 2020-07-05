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
    public class Accounts : ViewComponent, IDisposable
    {
        private readonly IAccountService _accountService;
        private bool _disposed = false;

        public Accounts(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                var accounts = new List<Account>();
                var user = await HttpContext.Session.GetJsonAsync<WebUser>(nameof(WebUser));
                
                if (user != null)
                {
                    var taskResult = await _accountService.GetListAsync(u => u.UserId == user.Id);
                    accounts = taskResult.ToList();
                }

                return View("/Views/NavLeft/_Accounts.cshtml", accounts);
            }
            catch (ServiceException e)
            {
                throw new WebUiException($"Ошибка в ViewComponent с названием {nameof(Accounts)} в методе {nameof(InvokeAsync)}", e);
            }
            catch (Exception ex)
            {
                throw new WebUiException($"Ошибка в ViewComponent с названием {nameof(Accounts)} в методе {nameof(InvokeAsync)}", ex);
            }
        }        

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _accountService.Dispose();
                }

                _disposed = true;
            }
        }
    }
}
