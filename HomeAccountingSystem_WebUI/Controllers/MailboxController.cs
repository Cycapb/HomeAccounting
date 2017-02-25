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
            ViewBag.PanelTitle = "Добавление почтового ящика";
            return View("Mailbox", model);
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
            return View("Mailbox");
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            await _mailboxService.DeleteAsync(id);
            return RedirectToAction("List");
        }

        public async Task<ActionResult> Edit(int id)
        {
            var model = await _mailboxService.GetItemAsync(id);

            if (model == null)
            {
                return RedirectToAction("Index");
            }

            var mailBoxModel = new MailboxAddViewModel()
            {
                Id = model.Id,
                MailBoxName = model.MailBoxName,
                MailFrom = model.MailFrom,
                Password = model.Password,
                PasswordConfirmation = string.Empty,
                Port = model.Port,
                Server = model.Server,
                UserName = model.UserName,
                UseSsl = model.UseSsl
            };

            ViewBag.PanelTitle = "Редактирование почтового ящика";
            return View("Mailbox", mailBoxModel);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(MailboxAddViewModel model)
        {
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                var itemToUpdate = await _mailboxService.GetItemAsync(model.Id);

                itemToUpdate.MailBoxName = model.MailBoxName;
                itemToUpdate.MailFrom = model.MailFrom;
                itemToUpdate.Password = model.Password;
                itemToUpdate.UserName = model.UserName;
                itemToUpdate.Server = model.Server;
                itemToUpdate.Port = model.Port;
                itemToUpdate.UseSsl = model.UseSsl;

                await _mailboxService.UpdateAsync(itemToUpdate);

                return RedirectToAction("Index");
            }
            return View("Mailbox");
        }
    }
    }
