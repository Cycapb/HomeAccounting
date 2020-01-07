using System;
using System.Threading.Tasks;
using DomainModels.Model;

namespace Services
{
    public interface ICreateCloseDebtService : IDisposable
    {
        Task CreateAsync(Debt debt);
        Task CloseAsync(int id, int accountId);
        Task PartialCloseAsync(int debtId, decimal sum, int accountId);
    }
}