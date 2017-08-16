using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModels.Exceptions;
using DomainModels.Model;
using DomainModels.Repositories;
using Services;
using Services.Exceptions;

namespace BussinessLogic.Services
{
    public class DebtService:IDebtService
    {
        private readonly IRepository<Debt> _debtRepo;
        private readonly IRepository<Account> _accRepo;

        public DebtService(IRepository<Debt> deptRepo, IRepository<Account> accRepo)
        {
            _debtRepo = deptRepo;
            _accRepo = accRepo;
        }
        public Task CreateAsync(Debt debt)
        {
            try
            {
                return Task.Run(async () =>
                {
                    await _debtRepo.CreateAsync(debt);
                    await _debtRepo.SaveAsync();
                    await CreateDebt(debt);
                });
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(DebtService)} в методе {nameof(CreateAsync)} при обращении к БД", e);
            }
        }

        public async Task CloseAsync(int id)
        {
            try
            {
                var item = await _debtRepo.GetItemAsync(id);
                item.DateEnd = DateTime.Now;
                await _debtRepo.UpdateAsync(item);
                await _debtRepo.SaveAsync();
                await CloseDebt(item);
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(DebtService)} в методе {nameof(CloseAsync)} при обращении к БД", e);
            }
        }

        public async Task<IEnumerable<Debt>> GetItemsAsync(string userId)
        {
            try
            {
                return (await _debtRepo.GetListAsync())
                    .Where(x => x.UserId == userId)
                    .ToList();
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(DebtService)} в методе {nameof(GetItemsAsync)} при обращении к БД", e);
            }
        }

        public async Task<Debt> GetItemAsync(int id)
        {
            try
            {
                return await _debtRepo.GetItemAsync(id);
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(DebtService)} в методе {nameof(GetItemAsync)} при обращении к БД", e);
            }
        }

        public IEnumerable<Debt> GetItems(string userId)
        {
            try
            {
                return _debtRepo.GetList().Where(x => x.UserId == userId);
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(DebtService)} в методе {nameof(GetItems)} при обращении к БД", e);
            }
        }

        public IEnumerable<Debt> GetOpenUserDebts(string userId)
        {
            try
            {
                return _debtRepo.GetList().Where(x => x.UserId == userId && x.DateEnd == null);
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(DebtService)} в методе {nameof(GetOpenUserDebts)} при обращении к БД", e);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                await _debtRepo.DeleteAsync(id);
                await _debtRepo.SaveAsync();
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(DebtService)} в методе {nameof(DeleteAsync)} при обращении к БД", e);
            }
        }

        private async Task CreateDebt(Debt debt)
        {
            try
            {
                var acc = await _accRepo.GetItemAsync(debt.AccountId);
                if (debt.TypeOfFlowId == 1)
                {
                    acc.Cash += debt.Summ;
                }
                else
                {
                    acc.Cash -= debt.Summ;
                }
                await _accRepo.UpdateAsync(acc);
                await _accRepo.SaveAsync();
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(DebtService)} в методе {nameof(CreateDebt)} при обращении к БД", e);
            }
        }

        private async Task CloseDebt(Debt debt)
        {
            try
            {
                var acc = await _accRepo.GetItemAsync(debt.AccountId);
                if (debt.TypeOfFlowId == 1)
                {
                    acc.Cash -= debt.Summ;
                }
                else
                {
                    acc.Cash += debt.Summ;
                }
                await _accRepo.UpdateAsync(acc);
                await _accRepo.SaveAsync();
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(DebtService)} в методе {nameof(CloseDebt)} при обращении к БД", e);
            }
        }
    }
}