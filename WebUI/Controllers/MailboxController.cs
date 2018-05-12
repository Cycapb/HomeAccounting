using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.SessionState;
using DomainModels.Model;
using WebUI.Models;
using Services;
using Services.Exceptions;
using WebUI.Exceptions;

namespace WebUI.Controllers
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
            try
            {
                var mailboxes = (await _mailboxService.GetListAsync()).ToList();
                return PartialView("_Index", mailboxes);
            }
            catch (ServiceException e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(MailboxController)} в методе {nameof(Index)}",
                    e);
            }
        }

        public async Task<ActionResult> List()
        {
            try
            {
                var mailboxes = (await _mailboxService.GetListAsync()).ToList();
                return PartialView("_List", mailboxes);
            }
            catch (ServiceException e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(MailboxController)} в методе {nameof(List)}",
                    e);
            }
        }

        public ActionResult Add()
        {
            var model = new MailboxAddViewModel();
            ViewBag.PanelTitle = "Добавление почтового ящика";
            return PartialView("_Mailbox", model);
        }

        [HttpPost]
        public async Task<ActionResult> Add(MailboxAddViewModel model)
        {
            if (ModelState.IsValid)
            {
                var box = new NotificationMailBox
                {
                    MailBoxName = model.MailBoxName,
                    MailFrom = model.MailFrom,
                    UserName = model.UserName,
                    Password = model.Password,
                    Server = model.Server,
                    Port = model.Port,
                    UseSsl = model.UseSsl
                };
                try
                {
                    await _mailboxService.AddAsync(box);
                    return RedirectToAction("Index");
                }
                catch (ServiceException e)
                {
                    throw new WebUiException($"Ошибка в контроллере {nameof(MailboxController)} в методе {nameof(Add)}",
                        e);
                }
            }

            return PartialView("_Mailbox");
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            await _mailboxService.DeleteAsync(id);
            return RedirectToAction("List");
        }

        public async Task<ActionResult> Edit(int id)
        {
            NotificationMailBox model;
            try
            {
                model = await _mailboxService.GetItemAsync(id);
            }
            catch (Exception e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(MailboxController)} в методе {nameof(Edit)}",
                    e);
            }

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
            return PartialView("_Mailbox", mailBoxModel);
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
                try
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
                }
                catch (Exception e)
                {
                    throw new WebUiException(
                        $"Ошибка в контроллере {nameof(MailboxController)} в методе {nameof(Edit)}", e);
                }

                return RedirectToAction("Index");
            }

            return PartialView("_Mailbox");
        }
    }
}