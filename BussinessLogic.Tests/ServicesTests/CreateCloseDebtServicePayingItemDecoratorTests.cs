using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BussinessLogic.Services;
using DomainModels.Model;
using DomainModels.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Services;

namespace BussinessLogic.Tests.ServicesTests
{
    [TestClass]
    public class CreateCloseDebtServicePayingItemDecoratorTests
    {
        private readonly Mock<IRepository<Category>> _categoryRepositoryMock;
        private readonly Mock<IRepository<PayingItem>> _payingItemRepositoryMock;
        private readonly Mock<IRepository<Debt>> _debtRepositoryMock;
        private readonly CreateCloseDebtServicePayingItemDecorator _target;
        private readonly Mock<ICreateCloseDebtService> _createCloseDebtServiceMock;
        private List<Debt> _listOfDebts;

        public CreateCloseDebtServicePayingItemDecoratorTests()
        {
            _categoryRepositoryMock = new Mock<IRepository<Category>>();                
            _payingItemRepositoryMock = new Mock<IRepository<PayingItem>>();
            _debtRepositoryMock = new Mock<IRepository<Debt>>();
            _createCloseDebtServiceMock = new Mock<ICreateCloseDebtService>();
            _target = new CreateCloseDebtServicePayingItemDecorator(_createCloseDebtServiceMock.Object, 
                _payingItemRepositoryMock.Object, 
                _categoryRepositoryMock.Object, 
                _debtRepositoryMock.Object);
            InitializeDebts();
        }

        [TestMethod]
        [TestCategory("CreateCloseDebtServicePayingItemDecoratorTests")]
        public async Task CreateAsync_CategoryExists_CreatesIncomingPayingItem()
        {
            var payingItem = new PayingItem();
            var debt = CreateIncomeDebt();
            _categoryRepositoryMock.Setup(m => m.GetListAsync()).ReturnsAsync(new List<Category>(){new Category(){Name = "долг", CategoryID = 1}});
            _payingItemRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<PayingItem>())).ReturnsAsync(new PayingItem())
                .Callback<PayingItem>(pi => payingItem = pi);

            await _target.CreateAsync(debt);

            Assert.AreEqual("Взял деньги в долг", payingItem.Comment);
        }

        [TestMethod]
        [TestCategory("CreateCloseDebtServicePayingItemDecoratorTests")]
        public async Task CreateAsync_CategoryExists_CreatesOutgoPayingItem()
        {
            var payingItem = new PayingItem();
            var debt = CreateOutgoingDebt();
            _categoryRepositoryMock.Setup(m => m.GetListAsync()).ReturnsAsync(new List<Category>() { new Category() { Name = "долг", CategoryID = 1 } });
            _payingItemRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<PayingItem>())).ReturnsAsync(new PayingItem())
                .Callback<PayingItem>(pi => payingItem = pi);

            await _target.CreateAsync(debt);

            Assert.AreEqual("Дал деньги в долг", payingItem.Comment);
        }

        [TestMethod]
        [TestCategory("CreateCloseDebtServicePayingItemDecoratorTests")]
        public async Task CreateAsync_CategoryNotExists_CreatesIncomePayingItem()
        {
            var payingItem = new PayingItem();
            var debt = CreateIncomeDebt();
            _categoryRepositoryMock.Setup(m => m.GetListAsync()).ReturnsAsync(new List<Category>(){new Category()
            {
                CategoryID = 1,
                Name = "Test"
            }});
            _categoryRepositoryMock.Setup(m => m.CreateAsync(It.IsAny<Category>())).ReturnsAsync(CreateCategory(debt));
            _payingItemRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<PayingItem>())).ReturnsAsync(new PayingItem())
                .Callback<PayingItem>(pi => payingItem = pi);

            await _target.CreateAsync(debt);

            Assert.AreEqual("Взял деньги в долг", payingItem.Comment);
        }

        [TestMethod]
        [TestCategory("CreateCloseDebtServicePayingItemDecoratorTests")]
        public async Task CloseAsync_CategoryExists_CreatesIncomePayingItemWithTypeOfFlow1()
        {
            var payingItem = new PayingItem();
            var debt = CreateIncomeDebt();
            _debtRepositoryMock.Setup(m => m.GetItemAsync(It.IsAny<int>())).ReturnsAsync(debt);
            _categoryRepositoryMock.Setup(m => m.GetListAsync()).ReturnsAsync(new List<Category>()
            {
                new Category()
                {
                    UserId = "1",
                    TypeOfFlowID = 2,
                    Name = "Долг",
                }
            });
            _payingItemRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<PayingItem>())).ReturnsAsync(new PayingItem())
                .Callback<PayingItem>(pi => payingItem = pi);

            await _target.CloseAsync(debt.DebtID);

            Assert.AreEqual("Закрыл свой долг", payingItem.Comment);
        }

        [TestMethod]
        [TestCategory("CreateCloseDebtServicePayingItemDecoratorTests")]
        public async Task CloseAsync_CategoryExists_CreatesOutgoPayingItemWithTypeOfFlow2()
        {
            var payingItem = new PayingItem();
            var debt = CreateOutgoingDebt();
            _debtRepositoryMock.Setup(m => m.GetItemAsync(It.IsAny<int>())).ReturnsAsync(debt);
            _categoryRepositoryMock.Setup(m => m.GetListAsync()).ReturnsAsync(new List<Category>()
            {
                new Category()
                {
                    UserId = "1",
                    TypeOfFlowID = 1,
                    Name = "Долг",
                }
            });
            _payingItemRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<PayingItem>())).ReturnsAsync(new PayingItem())
                .Callback<PayingItem>(pi => payingItem = pi);

            await _target.CloseAsync(debt.DebtID);

            Assert.AreEqual("Мне вернули долг", payingItem.Comment);
        }

        [TestMethod]
        [TestCategory("CreateCloseDebtServicePayingItemDecoratorTests")]
        public async Task PartialCloseAsync_CreatesPayingItem()
        {
            PayingItem payingItem = null;
            _debtRepositoryMock.Setup(x => x.GetItemAsync(It.IsAny<int>())).ReturnsAsync(_listOfDebts[0]);
            _payingItemRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<PayingItem>())).ReturnsAsync(new PayingItem())
                .Callback<PayingItem>(pi => payingItem = pi);
            _categoryRepositoryMock.Setup(m => m.GetListAsync()).ReturnsAsync(new List<Category>()
            {
                new Category()
                {
                    UserId = "1",
                    TypeOfFlowID = 1,
                    Name = "Долг",
                }
            });

            await _target.PartialCloseAsync(It.IsAny<int>(), 300);

            Assert.IsNotNull(payingItem);
        }

        [TestMethod]
        [TestCategory("CreateCloseDebtServicePayingItemDecoratorTests")]
        public async Task PartialCloseAsync_InputSum300_CreatesPayingItemWithSumm300()
        {
            PayingItem payingItem = null;
            _debtRepositoryMock.Setup(x => x.GetItemAsync(It.IsAny<int>())).ReturnsAsync(_listOfDebts[0]);
            
            _payingItemRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<PayingItem>()))
                .ReturnsAsync(new PayingItem())
                .Callback<PayingItem>(x => payingItem = x);
            _categoryRepositoryMock.Setup(m => m.GetListAsync()).ReturnsAsync(new List<Category>()
            {
                new Category()
                {
                    UserId = "1",
                    TypeOfFlowID = 1,
                    Name = "Долг",
                }
            });

            await _target.PartialCloseAsync(It.IsAny<int>(), 300);

            Assert.AreEqual(300, payingItem.Summ);
        }

        [TestMethod]
        [TestCategory("CreateCloseDebtServicePayingItemDecoratorTests")]
        public async Task PartialCloseAsync_DebtTypeIncome_CategoryDoesNotExist_InputSum300_CreatesPayingItemWithSumm300()
        {
            PayingItem payingItem = null;
            _debtRepositoryMock.Setup(x => x.GetItemAsync(It.IsAny<int>())).ReturnsAsync(_listOfDebts[0]);

            _payingItemRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<PayingItem>()))
                .ReturnsAsync(new PayingItem())
                .Callback<PayingItem>(x => payingItem = x);

            await _target.PartialCloseAsync(It.IsAny<int>(), 300);

            Assert.AreEqual(300, payingItem.Summ);
        }

        private Category CreateCategory(Debt debt)
        {
            return new Category()
            {
                Active = true,
                Name = "Долг",
                TypeOfFlowID = debt.TypeOfFlowId,
                UserId = debt.UserId,
                ViewInPlan = false,
                CategoryID = 2
            };
        }

        private Debt CreateIncomeDebt()
        {
            return new Debt()
            {
                AccountId = 1,
                TypeOfFlowId = 1,
                DateBegin = DateTime.Now,
                DebtID = 1,
                Person = "Income from Test",
                UserId = "1"
            };
        }

        private Debt CreateOutgoingDebt()
        {
            return new Debt()
            {
                AccountId = 1,
                TypeOfFlowId = 2,
                DateBegin = DateTime.Now,
                DebtID = 2,
                Person = "Outgo from Test",
                UserId = "1"
            };
        }

        private void InitializeDebts()
        {
            _listOfDebts = new List<Debt>()
            {
                new Debt()
                {
                    AccountId = 1,
                    DebtID = 1,
                    Summ = 500,
                    UserId = "1"
                }
            };
        }
    }
}
