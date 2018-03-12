using System;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Threading.Tasks;
using BussinessLogic.Services;
using DomainModels.Exceptions;
using DomainModels.Model;
using DomainModels.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Services.Exceptions;

namespace BussinessLogic.Tests.ServicesTests
{
    [TestClass]
    public class CreateCloseDebtServiceTests
    {
        private List<Debt> _listOfDebts;
        private List<Category> _listOfCategories;
        private readonly Mock<IRepository<Debt>> _debtRepositoryMock;
        private readonly CreateCloseDebtService _createCloseDebtService;
        private readonly Mock<IRepository<Account>> _accountRepositoryMock;
        
        public CreateCloseDebtServiceTests()
        {
            InitializeDebts();
            InitializeCategories();
            _debtRepositoryMock = new Mock<IRepository<Debt>>();
            _accountRepositoryMock = new Mock<IRepository<Account>>();
            _createCloseDebtService = new CreateCloseDebtService(_debtRepositoryMock.Object, _accountRepositoryMock.Object);
        }

        [TestMethod]
        [TestCategory("CreateCloseDebtServiceTests")]
        public async Task CloseAsync_InputSum200_DebtWas500_Becomes300()
        {
            _debtRepositoryMock.Setup(x => x.GetItemAsync(It.IsAny<int>())).ReturnsAsync(_listOfDebts[0]);

            await _createCloseDebtService.PartialCloseAsync(It.IsAny<int>(), 200);

            Assert.AreEqual(300, _listOfDebts[0].Summ);
        }

        [TestMethod]
        [TestCategory("CreateCloseDebtServiceTests")]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public async Task CloseAsync_InputSum600_DebtWas500_ThrowsArgumentOutOfrange()
        {
            _debtRepositoryMock.Setup(x => x.GetItemAsync(It.IsAny<int>())).ReturnsAsync(_listOfDebts[0]);

            await _createCloseDebtService.PartialCloseAsync(It.IsAny<int>(), 600);
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

        [TestMethod]
        [TestCategory("CreateCloseDebtServiceTests")]
        [ExpectedException(typeof(ServiceException))]
        public async Task CloseAsync_ThrowsServiceException()
        {
            _debtRepositoryMock.Setup(x => x.GetItemAsync(It.IsAny<int>())).Throws<DomainModelsException>();

            await _createCloseDebtService.PartialCloseAsync(It.IsAny<int>(), It.IsAny<decimal>());
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

        private void InitializeCategories()
        {
            _listOfCategories = new List<Category>()
            {
                new Category()
                {
                    CategoryID = 1,
                    TypeOfFlowID = 1,
                    UserId = "1",
                    Name = "Долг"
                }
            };
        }
    }
}
