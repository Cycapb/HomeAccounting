using DomainModels.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Exceptions;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Core.Exceptions;
using WebUI.Core.Models.MailboxModels;

namespace WebUI.Core.Controllers
{
    [Authorize(Roles = "Administrators")]
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

                return PartialView("_Index", mailboxes);
            }
            catch (ServiceException e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(MailboxController)} в методе {nameof(Index)}", e);
            }
        }

        public async Task<IActionResult> List()
        {
            try
            {
                var mailboxes = (await _mailboxService.GetListAsync()).ToList();

                return PartialView("_List", mailboxes);
            }
            catch (ServiceException e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(MailboxController)} в методе {nameof(List)}", e);
            }
        }

        public IActionResult Add()
        {
            var model = new AddNotificationMailboxModel();
            ViewBag.PanelTitle = "Добавление почтового ящика";

            return PartialView("_AddOrEdit", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AddNotificationMailboxModel model)
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
                    await _mailboxService.CreateAsync(box);

                    return RedirectToAction("Index");
                }
                catch (ServiceException e)
                {
                    throw new WebUiException($"Ошибка в контроллере {nameof(MailboxController)} в методе {nameof(Add)}", e);
                }
            }

            return PartialView("_AddOrEdit", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _mailboxService.DeleteAsync(id);

            return RedirectToAction("List");
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var model = await _mailboxService.GetItemAsync(id);

                if (model == null)
                {
                    return RedirectToAction("Index");
                }

                var mailBoxModel = new AddNotificationMailboxModel()
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

                return PartialView("_AddOrEdit", mailBoxModel);
            }
            catch (Exception e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(MailboxController)} в методе {nameof(Edit)}", e);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditNotificationMailboxModel model)
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

            return PartialView("_AddOrEdit", model);
        }

        protected override void Dispose(bool disposing)
        {
            _mailboxService.Dispose();

            base.Dispose(disposing);
        }
    }
}