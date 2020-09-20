using DomainModels.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Services;
using Services.Exceptions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebUI.Controllers;
using WebUI.Exceptions;
using WebUI.Models;

namespace WebUI.Tests.ControllersTests
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
            var model = ((PartialViewResult)result).Model as NotificationMailboxModel;

            Assert.IsNotNull(model);
        }

        [TestMethod]
        [TestCategory("MailboxControllerTests")]
        [ExpectedException(typeof(WebUiException))]
        public async Task Add_RaisesWebUiException()
        {
            _mailboxService.Setup(m => m.CreateAsync(It.IsAny<NotificationMailBox>())).Throws<ServiceException>();

            await _controller.Add(new NotificationMailboxModel());
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
        public async Task Delete()
        {
            var result = await _controller.Delete(It.IsAny<int>());
            var routeResults = ((RedirectToRouteResult)result).RouteValues;

            _mailboxService.Verify(x => x.DeleteAsync(It.IsAny<int>()), Times.Exactly(1));
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            Assert.AreEqual(routeResults["action"], "List");
        }

        [TestMethod]
        [TestCategory("MailboxControllerTests")]
        public async Task Edit_ReturnsMailboxPartialViewWithModel()
        {
            _mailboxService.Setup(x => x.GetItemAsync(It.IsAny<int>())).ReturnsAsync(new NotificationMailBox() { Id = 1, MailBoxName = "M1" });

            var result = await _controller.Edit(1);
            var model = ((PartialViewResult)result).Model as NotificationMailboxModel;

            _mailboxService.Verify(x => x.GetItemAsync(It.IsAny<int>()), Times.Exactly(1));
            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
            Assert.IsNotNull(model);
            Assert.AreEqual(model.MailBoxName, "M1");
        }

        [TestMethod]
        [TestCategory("MailboxControllerTests")]
        public async Task Edit_ReturnsRedirectToList_NullModel()
        {
            NotificationMailBox mBox = null;
            _mailboxService.Setup(x => x.GetItemAsync(It.IsAny<int>())).ReturnsAsync(mBox);

            var result = await _controller.Edit(It.IsAny<int>());
            var redirectResult = ((RedirectToRouteResult)result);

            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            Assert.AreEqual(redirectResult.RouteValues["action"], "Index");
        }

        [TestMethod]
        [TestCategory("MailboxControllerTests")]
        public async Task Edit_InputMailboxAddViewModelNull_ReturnsRedirectToList()
        {
            var result = await _controller.Edit(null);

            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }

        [TestMethod]
        [TestCategory("MailboxControllerTests")]
        public async Task Edit_InputmailboxAddViewModel_ReturnsRedirectToList()
        {
            var model = new NotificationMailboxModel() { Id = 2, MailBoxName = "M2" };
            _mailboxService.Setup(x => x.GetItemAsync(It.IsAny<int>())).ReturnsAsync(new NotificationMailBox() { Id = 1, MailBoxName = "M1" });

            var result = await _controller.Edit(model);
            var redirectResult = ((RedirectToRouteResult)result);

            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            Assert.AreEqual(redirectResult.RouteValues["action"], "Index");
            _mailboxService.Verify(x => x.UpdateAsync(It.IsAny<NotificationMailBox>()), Times.Exactly(1));
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

            await _controller.Edit(new NotificationMailboxModel());
        }
    }
}
