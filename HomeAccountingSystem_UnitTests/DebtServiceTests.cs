using System.Threading.Tasks;
using BussinessLogic.Services;
using HomeAccountingSystem_DAL.Model;
using HomeAccountingSystem_DAL.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Services;

namespace HomeAccountingSystem_UnitTests
{
    [TestClass]
    public class DebtServiceTests
    {
        private readonly Mock<IRepository<Debt>> _debtRepoMock;
        private readonly Mock<IRepository<Account>> _accRepoMock;
        private readonly IDebtService _service;

        public DebtServiceTests()
        {
            _debtRepoMock = new Mock<IRepository<Debt>>();
            _accRepoMock = new Mock<IRepository<Account>>();
            _service = new DebtService(_debtRepoMock.Object, _accRepoMock.Object);
        }

        [TestMethod]
        public async Task DeleteAsync()
        {
            await _service.DeleteAsync(It.IsAny<int>());

            _debtRepoMock.Verify(m => m.DeleteAsync(It.IsAny<int>()),Times.Exactly(1));
        }
    }
}
