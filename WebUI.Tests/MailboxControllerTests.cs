using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using System.Web;
using HomeAccountingSystem_WebUI.Controllers;
using Services;
using Moq;
using System.Collections.Generic;
using DomainModels.Model;
using System.Threading.Tasks;

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
    }
}
