﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using System.Web;
using HomeAccountingSystem_WebUI.Controllers;
using Services;
using Moq;
using System.Collections.Generic;
using DomainModels.Model;
using System.Threading.Tasks;
using HomeAccountingSystem_WebUI.Models;

namespace WebUI.Tests
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
        public async Task Index()
        {
            _mailboxService.Setup(m => m.GetListAsync()).ReturnsAsync(_list);

            var result = await _controller.Index();
            var model = ((ViewResult)result).Model as List<NotificationMailBox>;

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.AreEqual(2,model.Count);
        }

        [TestMethod]
        public void Add_ReturnsAddViewWithGET()
        {
            var result = _controller.Add();
            var model = ((PartialViewResult) result).Model as MailboxAddViewModel;

            Assert.IsNotNull(model);
        }

        [TestMethod]
        public async Task Delete()
        {
            var result = await _controller.Delete(It.IsAny<int>());
            var routeResults = ((RedirectToRouteResult) result).RouteValues;

            _mailboxService.Verify(x => x.DeleteAsync(It.IsAny<int>()), Times.Exactly(1));
            Assert.IsInstanceOfType(result,typeof(RedirectToRouteResult));
            Assert.AreEqual(routeResults["action"],"List");
        }

        [TestMethod]
        public async Task Edit_ReturnsMailboxViewWithModel()
        {
            _mailboxService.Setup(x => x.GetItemAsync(It.IsAny<int>())).ReturnsAsync(new NotificationMailBox() {Id = 1,MailBoxName = "M1"});

            var result = await _controller.Edit(1);
            var model = ((PartialViewResult) result).Model as MailboxAddViewModel;

            _mailboxService.Verify(x => x.GetItemAsync(It.IsAny<int>()), Times.Exactly(1));
            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
            Assert.IsNotNull(model);
            Assert.AreEqual(model.MailBoxName, "M1");
        }

        [TestMethod]
        public async Task Edit_ReturnsRedirectToList_NullModel()
        {
            _mailboxService.Setup(x => x.GetItemAsync(It.IsAny<int>())).ReturnsAsync(null);

            var result = await _controller.Edit(It.IsAny<int>());
            var redirectResult = ((RedirectToRouteResult) result);

            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            Assert.AreEqual(redirectResult.RouteValues["action"], "List");
        }
    }
}
