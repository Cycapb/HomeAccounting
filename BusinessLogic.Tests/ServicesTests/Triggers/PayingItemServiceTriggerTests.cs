using System.Threading.Tasks;
using BussinnessLogic.Services.Triggers;
using DomainModels.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Services;
using Services.Exceptions;

namespace BusinessLogic.Tests.ServicesTests.Triggers
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
        public async Task Update_OldSummGreaterThanNewSumm_TypeOfFlowId1()
        {
            var oldItem = new PayingItem() {Summ = 500};
            var newItem = new PayingItem()
            {
                Summ = 400,
                Category = new Category() {TypeOfFlowID = 1},
                Account = new Account() {Cash = 500}
            };
            var account = newItem.Account;
            var target = new PayingItemServiceTrigger(null, _accountService.Object);
            _accountService.Setup(x => x.GetItemAsync(It.IsAny<int>())).ReturnsAsync(account);

            await target.Update(oldItem, newItem);

            Assert.AreEqual(account.Cash, 400);
        }

        [TestMethod]
        [TestCategory("PayingItemServiceTriggerTests")]
        public async Task Update_NewSummGreaterThanOldSumm_TypeOfFlowId1()
        {
            var oldItem = new PayingItem() {Summ = 500};
            var newItem = new PayingItem()
            {
                Summ = 600,
                Category = new Category() {TypeOfFlowID = 1},
                Account = new Account() {Cash = 500}
            };
            var account = newItem.Account;
            var target = new PayingItemServiceTrigger(null, _accountService.Object);
            _accountService.Setup(x => x.GetItemAsync(It.IsAny<int>())).ReturnsAsync(account);

            await target.Update(oldItem, newItem);

            Assert.AreEqual(account.Cash, 600);
        }

        [TestMethod]
        [TestCategory("PayingItemServiceTriggerTests")]
        public async Task Update_OldSummGreaterThanNewSumm_TypeOfFlowId2()
        {
            var oldItem = new PayingItem() { Summ = 500 };
            var newItem = new PayingItem()
            {
                Summ = 400,
                Category = new Category() { TypeOfFlowID = 2 },
                Account = new Account() { Cash = 500 }
            };
            var account = newItem.Account;
            var target = new PayingItemServiceTrigger(null, _accountService.Object);
            _accountService.Setup(x => x.GetItemAsync(It.IsAny<int>())).ReturnsAsync(account);

            await target.Update(oldItem, newItem);

            Assert.AreEqual(account.Cash, 600);
        }

        [TestMethod]
        [TestCategory("PayingItemServiceTriggerTests")]
        public async Task Update_NewSummGreaterThanOldSumm_TypeOfFlowId2()
        {
            var oldItem = new PayingItem() { Summ = 500 };
            var newItem = new PayingItem()
            {
                Summ = 600,
                Category = new Category() { TypeOfFlowID = 2 },
                Account = new Account() { Cash = 500 }
            };
            var account = newItem.Account;
            var target = new PayingItemServiceTrigger(null, _accountService.Object);
            _accountService.Setup(x => x.GetItemAsync(It.IsAny<int>())).ReturnsAsync(account);

            await target.Update(oldItem, newItem);

            Assert.AreEqual(account.Cash, 400);
        }

        [TestMethod]
        [TestCategory("PayingItemServiceTriggerTests")]
        public async Task Update_SumChangedAndAccountChanged_TypeOfFlowId2()
        {
            var oldItem = new PayingItem() { Summ = 500, AccountID = 1, Account = new Account(){Cash = 1000}};
            var newItem = new PayingItem()
            {
                Summ = 600,
                Category = new Category() { TypeOfFlowID = 2 },
                Account = new Account() { Cash = 1000},
                AccountID = 2
            };
            var newAccount = newItem.Account;
            var oldAccount = oldItem.Account;
            var target = new PayingItemServiceTrigger(null, _accountService.Object);
            _accountService.Setup(x => x.GetItemAsync(newItem.AccountID)).ReturnsAsync(newAccount);
            _accountService.Setup(x => x.GetItemAsync(oldItem.AccountID)).ReturnsAsync(oldAccount);

            await target.Update(oldItem, newItem);

            Assert.AreEqual(oldAccount.Cash, 1500);
            Assert.AreEqual(newAccount.Cash, 400);
        }

        [TestMethod]
        [TestCategory("PayingItemServiceTriggerTests")]
        public async Task Update_SumChangedAndAccountChanged_TypeOfFlowId1()
        {
            var oldItem = new PayingItem() { Summ = 500, AccountID = 1, Account = new Account() { Cash = 1000 } };
            var newItem = new PayingItem()
            {
                Summ = 600,
                Category = new Category() { TypeOfFlowID = 1 },
                Account = new Account() { Cash = 1000 },
                AccountID = 2
            };
            var newAccount = newItem.Account;
            var oldAccount = oldItem.Account;
            var target = new PayingItemServiceTrigger(null, _accountService.Object);
            _accountService.Setup(x => x.GetItemAsync(newItem.AccountID)).ReturnsAsync(newAccount);
            _accountService.Setup(x => x.GetItemAsync(oldItem.AccountID)).ReturnsAsync(oldAccount);

            await target.Update(oldItem, newItem);

            Assert.AreEqual(oldAccount.Cash, 500);
            Assert.AreEqual(newAccount.Cash, 1600);
        }

        [TestMethod]
        [TestCategory("PayingItemServiceTriggerTests")]
        public async Task Update_SumChangedAndAccountChangedAndCategoryChanged_TypeOfFlowId2()
        {
            var oldItem = new PayingItem() { Summ = 500, AccountID = 1, Account = new Account() { Cash = 1000 }, CategoryID = 1};
            var newItem = new PayingItem()
            {
                Summ = 600,
                Category = new Category() { TypeOfFlowID = 2 },
                Account = new Account() { Cash = 1000 },
                AccountID = 2,
                CategoryID = 2
            };
            var newAccount = newItem.Account;
            var oldAccount = oldItem.Account;
            var target = new PayingItemServiceTrigger(null, _accountService.Object);
            _accountService.Setup(x => x.GetItemAsync(newItem.AccountID)).ReturnsAsync(newAccount);
            _accountService.Setup(x => x.GetItemAsync(oldItem.AccountID)).ReturnsAsync(oldAccount);

            await target.Update(oldItem, newItem);

            Assert.AreEqual(oldAccount.Cash, 1500);
            Assert.AreEqual(newAccount.Cash, 400);
        }
    }
}
