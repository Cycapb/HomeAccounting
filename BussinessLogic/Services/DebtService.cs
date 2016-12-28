using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeAccountingSystem_DAL.Model;
using HomeAccountingSystem_DAL.Repositories;
using Services;

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
            return Task.Run(async () =>
            {
                await _debtRepo.CreateAsync(debt);
                await _debtRepo.SaveAsync();
                await CreateDebt(debt);
            });
        }

        public async Task CloseAsync(int id)
        {
            var item = await _debtRepo.GetItemAsync(id);
            item.DateEnd = DateTime.Now;
            await _debtRepo.UpdateAsync(item);
            await _debtRepo.SaveAsync();
            await CloseDebt(item);

        }

        public async Task<IEnumerable<Debt>> GetItemsAsync(string userId)
        {
            return (await _debtRepo.GetListAsync())
                    .Where(x => x.UserId == userId)
                    .ToList();
        }

        public async Task<Debt> GetItemAsync(int id)
        {
            return await _debtRepo.GetItemAsync(id);
        }

        public IEnumerable<Debt> GetItems(string userId)
        {
            return _debtRepo.GetList().Where(x => x.UserId == userId);
        }

        public IEnumerable<Debt> GetOpenUserDebts(string userId)
        {
            return _debtRepo.GetList().Where(x => x.UserId == userId && x.DateEnd == null);
        }

        public async Task DeleteAsync(int id)
        {
            await _debtRepo.DeleteAsync(id);
            await _debtRepo.SaveAsync();
        }

        private async Task CreateDebt(Debt debt)
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

        private async Task CloseDebt(Debt debt)
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
    }
}