using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.SessionState;
using DomainModels.Model;
using HomeAccountingSystem_WebUI.Models;
using Services;

namespace HomeAccountingSystem_WebUI.Controllers
{
    [Authorize(Roles = "Administrators")]
    [SessionState(SessionStateBehavior.Disabled)]
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

        public async Task<ActionResult> List()
        {
            var mailboxes = (await _mailboxService.GetListAsync()).ToList();
            return PartialView("_List", mailboxes);
        }

        public ActionResult Add()
        {
            var model = new MailboxAddViewModel();
            return PartialView("_Add", model);
        }

        [HttpPost]
        public async Task<ActionResult> Add(MailboxAddViewModel model)
        {
            if (ModelState.IsValid)
            {
                var box = new NotificationMailBox
                {
                  MailBoxName  = model.MailBoxName,
                  MailFrom = model.MailFrom,
                  UserName = model .UserName,
                  Password = model.Password,
                  Server = model.Server,
                  Port = model.Port,
                  UseSsl  = model.UseSsl
                };
                await _mailboxService.AddAsync(box);
                return RedirectToAction("List");
            }
            return RedirectToAction("Add");
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            await _mailboxService.DeleteAsync(id);
            return RedirectToAction("List");
        }
    }
    }
