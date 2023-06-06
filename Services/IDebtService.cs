using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DomainModels.Model;
using Services.BaseInterfaces;

namespace Services
{
    public interface IDebtService : IQueryService<Debt>, IQueryServiceAsync<Debt>, IDisposable
    {
        IEnumerable<Debt> GetOpenUserDebts(string userId);

        Task<IEnumerable<Debt>> GetOpenUserDebtsAsync(string userId);

        Task DeleteAsync(int id);
    }
}
