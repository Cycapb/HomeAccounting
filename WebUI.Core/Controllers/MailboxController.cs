using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Exceptions;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Core.Controllers
{
    public class MailboxController : Controller
    {
        private readonly IMailboxService _mailboxService;

        public MailboxController(IMailboxService mailboxService)
        {
            _mailboxService = mailboxService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var mailboxes = (await _mailboxService.GetListAsync()).ToList();
                return Ok(mailboxes);
            }
            catch (ServiceException)
            {
                throw; //new WebUiException($"Ошибка в контроллере {nameof(MailboxController)} в методе {nameof(Index)}", e);
            }
        }
    }
}