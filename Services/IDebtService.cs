using System.Collections.Generic;
using System.Threading.Tasks;
using DomainModels.Model;

namespace Services
{
    public interface IDebtService : IQueryService<Debt>, IQueryServiceAsync<Debt>
    {
        IEnumerable<Debt> GetOpenUserDebts(string userId);

        Task DeleteAsync(int id);
    }
}
