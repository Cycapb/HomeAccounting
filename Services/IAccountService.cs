using DomainModels.Model;
using Services.BaseInterfaces;

namespace Services
{
    public interface IAccountService : IQueryService<Account>, IQueryServiceAsync<Account>, ICommandServiceAsync<Account>
    {
        bool HasAnyDependencies(int accountId);

        bool HasEnoughMoney(Account account, decimal summ);
    }
}