using System.Collections.Generic;
using System.Threading.Tasks;
using DomainModels.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Services;
using Services.Exceptions;
using WebUI.Core.Controllers;
using WebUI.Core.Exceptions;
using WebUI.Core.Models.MailboxModels;

namespace WebUI.Core.Tests.Controllers
{
    [TestClass]
    public class MailboxControllerTests
    {
        private readonly MailboxController _controller;
        private readonly Mock<IMailboxService> _mailboxService;
        private readonly List<NotificationMailBox> _list = new List<NotificationMailBox>()
        {
            new NotificationMailBox() { Id = 1, MailBoxName = "M1"},
            new NotificationMailBox() { Id = 2, MailBoxName = "M2"}
        };

        public MailboxControllerTests()
        {
            _mailboxService = new Mock<IMailboxService>();
            _controller = new MailboxController(_mailboxService.Object);
        }

        [TestMethod]
        [TestCategory("MailboxControllerTests")]
        public async Task Index()
        {
            _mailboxService.Setup(m => m.GetListAsync()).ReturnsAsync(_list);

            var result = await _controller.Index();
            var model = ((PartialViewResult)result).Model as List<NotificationMailBox>;

            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
            Assert.IsNotNull(model);
            Assert.AreEqual(2, model.Count);
        }

        [TestMethod]
        [TestCategory("MailboxControllerTests")]
        [ExpectedException(typeof(WebUiException))]
        public async Task Index_RaiseWebUiException()
        {
            _mailboxService.Setup(m => m.GetListAsync()).Throws<ServiceException>();

            await _controller.Index();
        }

        [TestMethod]
        [TestCategory("MailboxControllerTests")]
        public async Task Index_RaiseWebUiExceptionWithInnerServiceException()
        {
            _mailboxService.Setup(m => m.GetListAsync()).Throws<ServiceException>();

            try
            {
                await _controller.Index();
            }
            catch (WebUiException e)
            {
                Assert.IsInstanceOfType(e.InnerException, typeof(ServiceException));
            }

        }

        [TestMethod]
        [TestCategory("MailboxControllerTests")]
        public void Add_ReturnsAddViewWithGET()
        {
            var result = _controller.Add();
            var model = ((PartialViewResult)result).Model as AddNotificationMailboxModel;

            Assert.IsNotNull(model);
        }

        [TestMethod]
        [TestCategory("MailboxControllerTests")]
        [ExpectedException(typeof(WebUiException))]
        public async Task Add_RaisesWebUiException()
        {
            _mailboxService.Setup(m => m.CreateAsync(It.IsAny<NotificationMailBox>())).Throws<ServiceException>();

            await _controller.Add(new AddNotificationMailboxModel());
        }

        [TestMethod]
        [TestCategory("MailboxControllerTests")]
        [ExpectedException(typeof(WebUiException))]
        public async Task List_RaisesWebUiException()
        {
            _mailboxService.Setup(m => m.GetListAsync()).Throws<ServiceException>();

            await _controller.List();
        }

        [TestMethod]
        [TestCategory("MailboxControllerTests")]
        public async Task Delete_ReturnsRedirectToActionList()
        {
            var result = await _controller.Delete(It.IsAny<int>());
            
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            Assert.AreEqual("List", ((RedirectToActionResult)result).ActionName);
        }

        [TestMethod]
        [TestCategory("MailboxControllerTests")]
        public async Task Edit_ReturnsMailboxPartialViewWithModel()
        {
            _mailboxService.Setup(x => x.GetItemAsync(It.IsAny<int>())).ReturnsAsync(new NotificationMailBox() { Id = 1, MailBoxName = "M1" });

            var result = await _controller.Edit(1);
            var model = ((PartialViewResult)result).Model as AddNotificationMailboxModel;

            _mailboxService.Verify(x => x.GetItemAsync(It.IsAny<int>()), Times.Exactly(1));
            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
            Assert.IsNotNull(model);
            Assert.AreEqual(model.MailBoxName, "M1");
        }

        [TestMethod]
        [TestCategory("MailboxControllerTests")]
        public async Task Edit_ReturnsRedirectToList_NullModel()
        {
            _mailboxService.Setup(x => x.GetItemAsync(It.IsAny<int>())).ReturnsAsync((NotificationMailBox)null);

            var result = await _controller.Edit(It.IsAny<int>());
            var redirectResult = ((RedirectToActionResult)result);

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            Assert.AreEqual("Index", redirectResult.ActionName);
        }

        [TestMethod]
        [TestCategory("MailboxControllerTests")]
        public async Task Edit_InputEditNotificationMailboxModelNull_ReturnsRedirectToList()
        {
            var result = await _controller.Edit(null);

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
        }

        [TestMethod]
        [TestCategory("MailboxControllerTests")]
        public async Task Edit_InputValidEditNotificationMailboxModel_ReturnsRedirectToList()
        {
            var model = new EditNotificationMailboxModel() { Id = 2, MailBoxName = "M2" };
            _mailboxService.Setup(x => x.GetItemAsync(It.IsAny<int>())).ReturnsAsync(new NotificationMailBox() { Id = 1, MailBoxName = "M1" });

            var result = await _controller.Edit(model);
            var redirectResult = ((RedirectToActionResult)result);

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            Assert.AreEqual("Index", redirectResult.ActionName);
        }

        [TestMethod]
        [TestCategory("MailboxControllerTests")]
        [ExpectedException(typeof(WebUiException))]
        public async Task Edit_HttpGetMethod_RaisesWebUiException()
        {
            _mailboxService.Setup(m => m.GetItemAsync(It.IsAny<int>())).Throws<ServiceException>();

            await _controller.Edit(1);
        }

        [TestMethod]
        [TestCategory("MailboxControllerTests")]
        [ExpectedException(typeof(WebUiException))]
        public async Task Edit_HttpPostMethod_RaisesWebUiException()
        {
            _mailboxService.Setup(m => m.UpdateAsync(It.IsAny<NotificationMailBox>())).Throws<ServiceException>();

            await _controller.Edit(new EditNotificationMailboxModel());
        }
    }
}
