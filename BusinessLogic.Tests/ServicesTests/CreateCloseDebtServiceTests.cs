using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BussinnessLogic.Services;
using DomainModels.Exceptions;
using DomainModels.Model;
using DomainModels.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Services.Exceptions;

namespace BusinessLogic.Tests.ServicesTests
{
    [TestClass]
    public class CreateCloseDebtServiceTests
    {
        private readonly Mock<IRepository<Debt>> _debtRepositoryMock;
        private readonly Mock<IRepository<Account>> _accountRepositoryMock;
        private readonly CreateCloseDebtService _createCloseDebtService;
        
        public CreateCloseDebtServiceTests()
        {
            _debtRepositoryMock = new Mock<IRepository<Debt>>();
            _accountRepositoryMock = new Mock<IRepository<Account>>();
            _createCloseDebtService = new CreateCloseDebtService(_debtRepositoryMock.Object, _accountRepositoryMock.Object);
        }

        [TestMethod]
        [TestCategory("CreateCloseDebtServiceTests")]
        public async Task PartialCloseAsync_DebtTypeIncome_InputSum200_DebtWas500_Becomes300()
        {
            var debt = InitIncomingDebt();
            _debtRepositoryMock.Setup(x => x.GetItemAsync(It.IsAny<int>())).ReturnsAsync(debt);
            _accountRepositoryMock.Setup(x => x.GetItemAsync(It.IsAny<int>())).ReturnsAsync(new Account() { Cash = 500 });

            await _createCloseDebtService.PartialCloseAsync(It.IsAny<int>(), 200, It.IsAny<int>());

            Assert.AreEqual(300, debt.Summ);
        }

        [TestMethod]
        [TestCategory("CreateCloseDebtServiceTests")]
        public async Task PartialCloseAsync_DebtTypeOutgo_InputSum200_DebtWas500_Becomes300()
        {
            var debt = InitOutgoDebt();
            _debtRepositoryMock.Setup(x => x.GetItemAsync(It.IsAny<int>())).ReturnsAsync(debt);
            _accountRepositoryMock.Setup(x => x.GetItemAsync(It.IsAny<int>())).ReturnsAsync(new Account() { Cash = 500 });

            await _createCloseDebtService.PartialCloseAsync(It.IsAny<int>(), 200, It.IsAny<int>());

            Assert.AreEqual(300, debt.Summ);
        }

        [TestMethod]
        [TestCategory("CreateCloseDebtServiceTests")]
        public async Task PartialCloseAsync_DebtTypeIncome_InputSum200_AccountWas1000_Becomes800()
        {
            var debt = InitIncomingDebt();
            var account = InitAccount();
            _debtRepositoryMock.Setup(x => x.GetItemAsync(It.IsAny<int>())).ReturnsAsync(debt);
            _accountRepositoryMock.Setup(x => x.GetItemAsync(It.IsAny<int>())).ReturnsAsync(account);

            await _createCloseDebtService.PartialCloseAsync(It.IsAny<int>(), 200, It.IsAny<int>());

            Assert.AreEqual(800, account.Cash);
        }

        [TestMethod]
        [TestCategory("CreateCloseDebtServiceTests")]
        public async Task PartialCloseAsync_DebtTypeOutgo_InputSum200_AccountWas1000_Becomes1200()
        {
            var debt = InitOutgoDebt();
            var account = InitAccount();
            _debtRepositoryMock.Setup(x => x.GetItemAsync(It.IsAny<int>())).ReturnsAsync(debt);
            _accountRepositoryMock.Setup(x => x.GetItemAsync(It.IsAny<int>())).ReturnsAsync(account);

            await _createCloseDebtService.PartialCloseAsync(It.IsAny<int>(), 200, It.IsAny<int>());

            Assert.AreEqual(1200, account.Cash);
        }

        [TestMethod]
        [TestCategory("CreateCloseDebtServiceTests")]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public async Task PartialCloseAsync_InputSum600_DebtWas500_ThrowsArgumentOutOfRange()
        {
            _debtRepositoryMock.Setup(x => x.GetItemAsync(It.IsAny<int>())).ReturnsAsync(InitIncomingDebt());

            await _createCloseDebtService.PartialCloseAsync(It.IsAny<int>(), 600, It.IsAny<int>());
        }

        [TestMethod]
        [TestCategory("CreateCloseDebtServiceTests")]
        [ExpectedException(typeof(ServiceException))]
        public async Task PartialCloseAsync_ThrowsServiceException()
        {
            _debtRepositoryMock.Setup(x => x.GetItemAsync(It.IsAny<int>())).Throws<DomainModelsException>();

            await _createCloseDebtService.PartialCloseAsync(It.IsAny<int>(), It.IsAny<decimal>(), It.IsAny<int>());
        }

        [TestMethod]
        [TestCategory("CreateCloseDebtServiceTests")]
        public async Task PartialCloseAsync_RemovesMoneyFromChosenAccount()
        {
            var accountList = new List<Account>()
            {
                new Account()
                {
                    AccountID = 1,
                    Cash = 1000M,
                    AccountName = "Test1"
                },
                new Account()
                {
                    AccountID = 2,
                    Cash = 2000M,
                    AccountName = "Test2"
                }
            };
            var debt = InitIncomingDebt();
            _accountRepositoryMock.Setup(m => m.GetListAsync()).ReturnsAsync(accountList);
            _accountRepositoryMock.Setup(x => x.GetItemAsync(2)).ReturnsAsync(accountList.FirstOrDefault(x => x.AccountID == 2));
            _debtRepositoryMock.Setup(x => x.GetItemAsync(It.IsAny<int>())).ReturnsAsync(debt);

            debt.AccountId = 2;
            await _createCloseDebtService.PartialCloseAsync(debt.DebtID, 200, 2);

            Assert.AreEqual(1000M, accountList[0].Cash);
            Assert.AreEqual(1800M, accountList[1].Cash);
        }

        private Debt InitIncomingDebt()
        {
            return new Debt()
            {
                AccountId = 1,
                DebtID = 1,
                Summ = 500,
                UserId = "1",
                TypeOfFlowId = 1
            };
        }

        private Debt InitOutgoDebt()
        {
            return new Debt()
            {
                AccountId = 1,
                DebtID = 1,
                Summ = 500,
                UserId = "1",
                TypeOfFlowId = 2
            };
        }

        private Account InitAccount()
        {
            return new Account()
            {
                AccountID = 1,
                Cash = 1000
            };
        }
    }
}
