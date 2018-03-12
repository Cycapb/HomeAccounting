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
    public class CreateCloseDebtServiceDecoratorTests
    {
        private readonly Mock<IRepository<Category>> _categoryRepositoryMock;
        private readonly Mock<IRepository<PayingItem>> _payingItemRepositoryMock;
        private readonly Mock<IRepository<Debt>> _debtRepositoryMock;
        private readonly CreateCloseDebtServicePayingItemDecorator _target;
        private readonly Mock<ICreateCloseDebtService> _createCloseDebtServiceMock;

        public CreateCloseDebtServiceDecoratorTests()
        {
            _categoryRepositoryMock = new Mock<IRepository<Category>>();                
            _payingItemRepositoryMock = new Mock<IRepository<PayingItem>>();
            _debtRepositoryMock = new Mock<IRepository<Debt>>();
            _createCloseDebtServiceMock = new Mock<ICreateCloseDebtService>();
            _target = new CreateCloseDebtServicePayingItemDecorator(_createCloseDebtServiceMock.Object, 
                _payingItemRepositoryMock.Object, 
                _categoryRepositoryMock.Object, 
                _debtRepositoryMock.Object);
        }

        [TestMethod]
        [TestCategory("CreateCloseDebtServiceDecoratorTests")]
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
        [TestCategory("CreateCloseDebtServiceDecoratorTests")]
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
        [TestCategory("CreateCloseDebtServiceDecoratorTests")]
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
        [TestCategory("CreateCloseDebtServiceDecoratorTests")]
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
        [TestCategory("CreateCloseDebtServiceDecoratorTests")]
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

        //[TestMethod]
        //[TestCategory("CreateCloseDebtServiceTests")]
        //public async Task CloseAsync_CreatesPayingItem()
        //{
        //    PayingItem payingItem = null;
        //    _debtRepositoryMock.Setup(x => x.GetItemAsync(It.IsAny<int>())).ReturnsAsync(_listOfDebts[0]);

        //    await _createCloseDebtService.PartialCloseAsync(It.IsAny<int>(), 300);

        //    Assert.IsNotNull(payingItem);
        //}

        //[TestMethod]
        //[TestCategory("CreateCloseDebtServiceTests")]
        //public async Task CloseAsync_InputSum300_CreatesPayingItemWithSumm300()
        //{
        //    PayingItem payingItem = null;
        //    _debtRepositoryMock.Setup(x => x.GetItemAsync(It.IsAny<int>())).ReturnsAsync(_listOfDebts[0]);
        //    _categoryRepositoryMock.Setup(x => x.GetListAsync()).ReturnsAsync(_listOfCategories);
        //    _payingItemRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<PayingItem>()))
        //        .ReturnsAsync(new PayingItem())
        //        .Callback<PayingItem>(x => payingItem = x);

        //    await _debtServicePartialCloser.CloseAsync(It.IsAny<int>(), 300);

        //    Assert.AreEqual(300, payingItem.Summ);
        //}

        //[TestMethod]
        //[TestCategory("CreateCloseDebtServiceTests")]
        //public async Task CloseAsync_FoundDebtIsNull_NoPayingItemIsCreated()
        //{
        //    PayingItem payingItem = null;
        //    _debtRepositoryMock.Setup(x => x.GetItemAsync(It.IsAny<int>())).ReturnsAsync(null);
        //    _categoryRepositoryMock.Setup(x => x.GetListAsync()).ReturnsAsync(_listOfCategories);
        //    _payingItemRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<PayingItem>()))
        //        .ReturnsAsync(new PayingItem())
        //        .Callback<PayingItem>(x => payingItem = x);

        //    await _debtServicePartialCloser.CloseAsync(It.IsAny<int>(), 300);

        //    Assert.IsNull(payingItem);
        //}

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
    }
}
