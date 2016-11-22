using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeAccountingSystem_DAL.Model;
using HomeAccountingSystem_DAL.Repositories;
using Services;

namespace BussinessLogic.Services
{
    public class AccountService:IAccountService
    {
        private readonly IRepository<Account> _accountRepository;

        public AccountService(IRepository<Account> accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task Create(Account item)
        {
            await _accountRepository.CreateAsync(item);
            await _accountRepository.SaveAsync();
        }

        public async Task<Account> GetItemAsync(int id)
        {
            return await _accountRepository.GetItemAsync(id);
        }

        public async Task<IEnumerable<Account>> GetListAsync()
        {
            return await _accountRepository.GetListAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await _accountRepository.DeleteAsync(id);
            await _accountRepository.SaveAsync();
        }

        public async Task UpdateAsync(Account item)
        {
            await _accountRepository.UpdateAsync(item);
            await _accountRepository.SaveAsync();
        }

        public bool HasAnyDependencies(int accountId)
        {
            return _accountRepository.GetItem(accountId).PayingItem.Any();
        }

        public bool HasEnoughMoney(Account account, decimal summ)
        {
            var accSumm = summ;
            return account.Cash > 0 && (account.Cash - accSumm >= 0);
        }
    }
}