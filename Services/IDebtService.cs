using System.Collections.Generic;
using System.Threading.Tasks;
using HomeAccountingSystem_DAL.Model;

namespace Services
{
    public interface IDebtService
    {
        Task CreateAsync(Debt debt);
        Task CloseAsync(int id);
        Task<IEnumerable<Debt>> GetItemsAsync(string userId);
        Task<Debt> GetItemAsync(int id);
        IEnumerable<Debt> GetItems(string userId);
        IEnumerable<Debt> GetOpenUserDebts(string userId);
        Task DeleteAsync(int id);
    }
}
