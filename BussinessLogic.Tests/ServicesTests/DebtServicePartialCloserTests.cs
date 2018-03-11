using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BussinessLogic.Services;
using DomainModels.Model;
using DomainModels.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BussinessLogic.Tests.ServicesTests
{
    [TestClass]
    public class DebtServicePartialCloserTests
    {
        private List<Debt> _listOfDebts;
        private readonly Mock<IRepository<Debt>> _debtRepositoryMock;
        private readonly DebtServicePartialCloser _debtServicePartialCloser;

        public DebtServicePartialCloserTests()
        {
            InitializeDebts();
            _debtRepositoryMock = new Mock<IRepository<Debt>>();
            _debtServicePartialCloser = new DebtServicePartialCloser(_debtRepositoryMock.Object);
        }

        [TestMethod]
        [TestCategory("DebtServicePartialCloser")]
        public async Task CloseAsync_InputSum200_DebtWas500_Becomes300()
        {
            _debtRepositoryMock.Setup(x => x.GetItemAsync(It.IsAny<int>())).ReturnsAsync(_listOfDebts[0]);

            await _debtServicePartialCloser.CloseAsync(It.IsAny<int>(), 200);

            Assert.AreEqual(300, _listOfDebts[0].Summ);
        }

        [TestMethod]
        [TestCategory("DebtServicePartialCloser")]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public async Task CloseAsync_InputSum600_DebtWas500_ThrowsArgumentOutOfrange()
        {
            _debtRepositoryMock.Setup(x => x.GetItemAsync(It.IsAny<int>())).ReturnsAsync(_listOfDebts[0]);

            await _debtServicePartialCloser.CloseAsync(It.IsAny<int>(), 600);
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
