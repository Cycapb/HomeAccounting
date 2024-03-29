﻿using DomainModels.Exceptions;
using DomainModels.Model;
using DomainModels.Repositories;
using Services;
using Services.Exceptions;
using System;
using System.Threading.Tasks;

namespace BussinnessLogic.Services
{
    public class CreateCloseDebtService : ICreateCloseDebtService
    {
        private readonly IRepository<Debt> _debtRepo;
        private readonly IRepository<Account> _accRepo;
        private bool _disposed;

        public CreateCloseDebtService(IRepository<Debt> debtRepo, IRepository<Account> accRepo)
        {
            _debtRepo = debtRepo;
            _accRepo = accRepo;
        }

        public async Task CreateAsync(Debt debt)
        {
            try
            {
                _debtRepo.Create(debt);
                await _debtRepo.SaveAsync();
                await CreateDebtAsync(debt);
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(CreateCloseDebtService)} в методе {nameof(CreateAsync)} при обращении к БД", e);
            }
        }

        public async Task CloseAsync(int id, int accountId)
        {
            try
            {
                var debt = await _debtRepo.GetItemAsync(id);
                debt.DateEnd = DateTime.Now;
                debt.AccountId = accountId;
                _debtRepo.Update(debt);
                await _debtRepo.SaveAsync();
                await ChangeAccountMoney(debt, debt.Summ);
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(CreateCloseDebtService)} в методе {nameof(CloseAsync)} при обращении к БД", e);
            }
        }

        public async Task PartialCloseAsync(int debtId, decimal sum, int accountId)
        {
            Debt debt;
            try
            {
                debt = await _debtRepo.GetItemAsync(debtId);
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в типе {nameof(CreateCloseDebtService)} в методе {nameof(CloseAsync)}", e);
            }

            if (debt == null)
            {
                return;
            }
            if (sum > debt.Summ)
            {
                throw new ArgumentOutOfRangeException(nameof(sum), "Введенная сумма больше суммы долга");
            }
            debt.Summ -= sum;
            debt.AccountId = accountId;
            await ChangeAccountMoney(debt, sum);
            
            if (debt.Summ == 0M)
            {
                debt.DateEnd = DateTime.Now;
            }
            
            _debtRepo.Update(debt);
            await _debtRepo.SaveAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _debtRepo.Dispose();
                    _accRepo.Dispose();
                }

                _disposed = true;
            }
        }

        private async Task CreateDebtAsync(Debt debt)
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

                _accRepo.Update(acc);
                await _accRepo.SaveAsync();
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(CreateCloseDebtService)} в методе {nameof(CreateDebtAsync)} при обращении к БД", e);
            }
        }

        private async Task ChangeAccountMoney(Debt debt, decimal sum)
        {
            try
            {
                var acc = await _accRepo.GetItemAsync(debt.AccountId);
                if (debt.TypeOfFlowId == 1)
                {
                    acc.Cash -= sum;
                }
                else
                {
                    acc.Cash += sum;
                }
                _accRepo.Update(acc);
                await _accRepo.SaveAsync();
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(CreateCloseDebtService)} в методе {nameof(ChangeAccountMoney)} при обращении к БД", e);
            }
        }
    }
}