using DomainModels.Exceptions;
using DomainModels.Model;
using DomainModels.Repositories;
using Services;
using Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BussinnessLogic.Services
{
    public class DebtService : IDebtService
    {
        private readonly IRepository<Debt> _debtRepo;
        private bool _disposed;

        public DebtService(IRepository<Debt> deptRepo)
        {
            _debtRepo = deptRepo;
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

        public IEnumerable<Debt> GetList()
        {
            try
            {
                return _debtRepo.GetList();
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(DebtService)} в методе {nameof(GetList)} при обращении к БД", e);
            }
        }

        public IEnumerable<Debt> GetOpenUserDebts(string userId)
        {
            try
            {
                return _debtRepo.GetList(x => x.UserId == userId && x.DateEnd == null);
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

        public IEnumerable<Debt> GetList(Expression<Func<Debt, bool>> predicate)
        {
            try
            {
                return _debtRepo.GetList(predicate);
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(DebtService)} в методе {nameof(GetList)} при обращении к БД", e);
            }
        }

        public Debt GetItem(int id)
        {
            try
            {
                return _debtRepo.GetItem(id);
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(DebtService)} в методе {nameof(GetItem)} при обращении к БД", e);
            }
        }

        public async Task<IEnumerable<Debt>> GetListAsync()
        {
            try
            {
                return await _debtRepo.GetListAsync();
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(DebtService)} в методе {nameof(GetListAsync)} при обращении к БД", e);
            }
        }

        public async Task<IEnumerable<Debt>> GetListAsync(Expression<Func<Debt, bool>> predicate)
        {
            try
            {
                return await _debtRepo.GetListAsync(predicate);
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(DebtService)} в методе {nameof(GetListAsync)} при обращении к БД", e);
            }
        }

        public async Task<IEnumerable<Debt>> GetOpenUserDebtsAsync(string userId)
        {
            try
            {
                return await _debtRepo.GetListAsync(x => x.UserId == userId && x.DateEnd == null);
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(DebtService)} в методе {nameof(GetOpenUserDebts)} при обращении к БД", e);
            }
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
                }

                _disposed = true;
            }
        }
    }
}