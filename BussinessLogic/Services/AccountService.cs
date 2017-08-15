using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Threading.Tasks;
using DomainModels.Model;
using DomainModels.Repositories;
using DomainModels.Exceptions;
using Services;
using Services.Exceptions;

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
            try
            {
                await _accountRepository.CreateAsync(item);
                await _accountRepository.SaveAsync();
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(AccountService)} в методе {nameof(Create)} при обращении к БД", e);
            }
        }

        public async Task<Account> GetItemAsync(int id)
        {
            try
            {
                return await _accountRepository.GetItemAsync(id);
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(AccountService)} в методе {nameof(GetItemAsync)} при обращении к БД", e);
            }
        }

        public async Task<IEnumerable<Account>> GetListAsync()
        {
            try
            {
                return await _accountRepository.GetListAsync();
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(AccountService)} в методе {nameof(GetListAsync)} при обращении к БД", e);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                await _accountRepository.DeleteAsync(id);
                await _accountRepository.SaveAsync();
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(AccountService)} в методе {nameof(DeleteAsync)} при обращении к БД", e);
            }
        }

        public async Task UpdateAsync(Account item)
        {
            try
            {
                await _accountRepository.UpdateAsync(item);
                await _accountRepository.SaveAsync();
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(AccountService)} в методе {nameof(UpdateAsync)} при обращении к БД", e);
            }
        }

        public bool HasAnyDependencies(int accountId)
        {
            try
            {
                return _accountRepository.GetItem(accountId).PayingItem.Any();
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(AccountService)} в методе {nameof(HasAnyDependencies)} при обращении к БД", e);
            }
        }

        public bool HasEnoughMoney(Account account, decimal summ)
        {
            var accSumm = summ;
            return account.Cash > 0 && (account.Cash - accSumm >= 0);
        }

        public IEnumerable<Account> GetList()
        {
            try
            {
                return _accountRepository.GetList();
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(AccountService)} в методе {nameof(GetList)} при обращении к БД", e);
            }
        }
    }
}