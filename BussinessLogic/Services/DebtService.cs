using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        public DebtService(IRepository<Debt> deptRepo)
        {
            _debtRepo = deptRepo;
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
    }
}