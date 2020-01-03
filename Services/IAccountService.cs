using DomainModels.Model;
using Services.BaseInterfaces;
using System.Threading.Tasks;

namespace Services
{
    public interface IAccountService : IQueryService<Account>, IQueryServiceAsync<Account>
    {
        Task CreateAsync(Account item);

        Task DeleteAsync(int id);

        Task UpdateAsync(Account item);

        bool HasAnyDependencies(int accountId);

        bool HasEnoughMoney(Account account, decimal summ);
    }
}