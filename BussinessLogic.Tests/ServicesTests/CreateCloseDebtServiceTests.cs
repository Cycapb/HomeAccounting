﻿using System;
using System.Collections.Generic;
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
        private readonly Mock<IRepository<Debt>> _debtRepositoryMock;
        private readonly CreateCloseDebtService _createCloseDebtService;
        
        public CreateCloseDebtServiceTests()
        {
            InitializeDebts();
            _debtRepositoryMock = new Mock<IRepository<Debt>>();
            _createCloseDebtService = new CreateCloseDebtService(_debtRepositoryMock.Object, null);
        }

        [TestMethod]
        [TestCategory("CreateCloseDebtServiceTests")]
        public async Task PartialCloseAsync_InputSum200_DebtWas500_Becomes300()
        {
            _debtRepositoryMock.Setup(x => x.GetItemAsync(It.IsAny<int>())).ReturnsAsync(_listOfDebts[0]);

            await _createCloseDebtService.PartialCloseAsync(It.IsAny<int>(), 200);

            Assert.AreEqual(300, _listOfDebts[0].Summ);
        }

        [TestMethod]
        [TestCategory("CreateCloseDebtServiceTests")]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public async Task PartialCloseAsync_InputSum600_DebtWas500_ThrowsArgumentOutOfrange()
        {
            _debtRepositoryMock.Setup(x => x.GetItemAsync(It.IsAny<int>())).ReturnsAsync(_listOfDebts[0]);

            await _createCloseDebtService.PartialCloseAsync(It.IsAny<int>(), 600);
        }

        [TestMethod]
        [TestCategory("CreateCloseDebtServiceTests")]
        [ExpectedException(typeof(ServiceException))]
        public async Task PartialCloseAsync_ThrowsServiceException()
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
    }
}