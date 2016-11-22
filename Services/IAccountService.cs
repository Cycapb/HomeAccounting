using System.Collections.Generic;
using System.Threading.Tasks;
using HomeAccountingSystem_DAL.Model;

namespace Services
{
    public interface IAccountService
    {
        Task Create(Account item);
        Task<Account> GetItemAsync(int id);
        Task<IEnumerable<Account>> GetListAsync();
        Task DeleteAsync(int id);
        Task UpdateAsync(Account item);
        bool HasAnyDependencies(int accountId);
        bool HasEnoughMoney(Account account, decimal summ);
    }
}