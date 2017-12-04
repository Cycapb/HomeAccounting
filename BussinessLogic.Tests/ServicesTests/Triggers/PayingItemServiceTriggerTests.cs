using System.Threading.Tasks;
using BussinessLogic.Services.Triggers;
using DomainModels.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Services;
using Services.Exceptions;

namespace BussinessLogic.Tests.ServicesTests.Triggers
{
    [TestClass]
    public class PayingItemServiceTriggerTests
    {
        private readonly Mock<IAccountService> _accountService;
        private readonly Mock<ICategoryService> _categoryService;

        public PayingItemServiceTriggerTests()
        {
            _accountService = new Mock<IAccountService>();
            _categoryService = new Mock<ICategoryService>();
        }

        [TestMethod]
        [TestCategory("PayingItemServiceTriggerTests")]
        [ExpectedException(typeof(ServiceException))]
        public async Task Insert_CategoryServiceThrowsServiceException()
        {
            _categoryService.Setup(m => m.GetItemAsync(It.IsAny<int>())).Throws<ServiceException>();
            var target = new PayingItemServiceTrigger(_categoryService.Object, _accountService.Object);

            await target.Insert(new PayingItem());
        }

        [TestMethod]
        [TestCategory("PayingItemServiceTriggerTests")]
        [ExpectedException(typeof(ServiceException))]
        public async Task Insert_AccountServiceThrowsServiceException()
        {
            _categoryService.Setup(m => m.GetItemAsync(It.IsAny<int>()))
                .ReturnsAsync(new Category {TypeOfFlowID = 1});
            _accountService.Setup(m => m.GetItemAsync(It.IsAny<int>())).Throws<ServiceException>();
            var target = new PayingItemServiceTrigger(_categoryService.Object, _accountService.Object);

            await target.Insert(new PayingItem());
        }

        [TestMethod]
        [TestCategory("PayingItemServiceTriggerTests")]
        public async Task Insert_TypeOfFlowId1_AccountCashIncrease()
        {
            var insertedItem = new PayingItem(){Summ = 200};
            var account = new Account(){Cash = 500};
            _categoryService.Setup(m => m.GetItemAsync(It.IsAny<int>()))
                .ReturnsAsync(new Category { TypeOfFlowID = 1 });
            _accountService.Setup(m => m.GetItemAsync(It.IsAny<int>())).ReturnsAsync(account);
            var target = new PayingItemServiceTrigger(_categoryService.Object, _accountService.Object);

            await target.Insert(insertedItem);
            
            Assert.AreEqual(account.Cash, 700);
        }

        [TestMethod]
        [TestCategory("PayingItemServiceTriggerTests")]
        public async Task Insert_TypeOfFlowId2_AccountCashDecrease()
        {
            var insertedItem = new PayingItem() { Summ = 200 };
            var account = new Account() { Cash = 500 };
            _categoryService.Setup(m => m.GetItemAsync(It.IsAny<int>()))
                .ReturnsAsync(new Category { TypeOfFlowID = 2 });
            _accountService.Setup(m => m.GetItemAsync(It.IsAny<int>())).ReturnsAsync(account);
            var target = new PayingItemServiceTrigger(_categoryService.Object, _accountService.Object);

            await target.Insert(insertedItem);

            Assert.AreEqual(account.Cash, 300);
        }

        [TestMethod]
        [TestCategory("PayingItemServiceTriggerTests")]
        public async Task Delete_TypeOfFlowId1_AccountCashDecrease()
        {
            var deletedItem = new PayingItem()
            {
                Summ = 200,
                Account = new Account() { Cash = 500},
                Category = new Category() { TypeOfFlowID = 1}
            };
            var account = deletedItem.Account;
            _categoryService.Setup(m => m.GetItemAsync(It.IsAny<int>()))
                .ReturnsAsync(new Category { TypeOfFlowID = 1 });
            _accountService.Setup(m => m.GetItemAsync(It.IsAny<int>())).ReturnsAsync(account);
            var target = new PayingItemServiceTrigger(_categoryService.Object, _accountService.Object);

            await target.Delete(deletedItem);

            Assert.AreEqual(account.Cash, 300);
        }

        [TestMethod]
        [TestCategory("PayingItemServiceTriggerTests")]
        public async Task Delete_TypeOfFlowId2_AccountCashIncrease()
        {
            var deletedItem = new PayingItem()
            {
                Summ = 200,
                Account = new Account() { Cash = 500},
                Category = new Category() { TypeOfFlowID = 2 }
            };
            var account = deletedItem.Account;
            _categoryService.Setup(m => m.GetItemAsync(It.IsAny<int>()))
                .ReturnsAsync(new Category { TypeOfFlowID = 2 });
            _accountService.Setup(m => m.GetItemAsync(It.IsAny<int>())).ReturnsAsync(account);
            var target = new PayingItemServiceTrigger(_categoryService.Object, _accountService.Object);

            await target.Delete(deletedItem);

            Assert.AreEqual(account.Cash, 700);
        }

        [TestMethod]
        [TestCategory("PayingItemServiceTriggerTests")]
        public async Task Update_OldSummGreaterThanNewSumm()
        {
            var oldItem = new PayingItem(){Summ = 500};
            var newItem = new PayingItem(){Summ = 400, Account = new Account(){Cash = 500}};
            var account = newItem.Account;
            var target = new PayingItemServiceTrigger(null, _accountService.Object);

            await target.Update(oldItem, newItem);

            Assert.AreEqual(account.Cash, 400);
        }

        [TestMethod]
        [TestCategory("PayingItemServiceTriggerTests")]
        public async Task Update_NewSummGreaterThanOldSumm()
        {
            var oldItem = new PayingItem() { Summ = 500 };
            var newItem = new PayingItem() { Summ = 600, Account = new Account() { Cash = 500 } };
            var account = newItem.Account;
            var target = new PayingItemServiceTrigger(null, _accountService.Object);

            await target.Update(oldItem, newItem);

            Assert.AreEqual(account.Cash, 600);
        }
    }
}
