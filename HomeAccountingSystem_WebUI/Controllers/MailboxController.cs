using System.Linq;
using System.Web.Mvc;
using Services;
using System.Threading.Tasks;

namespace HomeAccountingSystem_WebUI.Controllers
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
            var mailboxes = (await _mailboxService.GetListAsync()).ToList();
            return View(mailboxes);
        }
    }
}