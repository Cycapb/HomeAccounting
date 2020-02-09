using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Exceptions;

namespace WebUI.Core.Controllers
{
    public class MailboxController : Controller
    {
        private readonly IMailboxService _mailboxService;

        public MailboxController(IMailboxService mailboxService)
        {
            _mailboxService = mailboxService;
        }

        public async Task<ActionResult> Index()
        {
            try
            {
                var mailboxes = (await _mailboxService.GetListAsync()).ToList();
                return PartialView("_Index", mailboxes);
            }
            catch (ServiceException e)
            {
                throw; //new WebUiException($"Ошибка в контроллере {nameof(MailboxController)} в методе {nameof(Index)}", e);
            }
        }
    }
}