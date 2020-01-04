using DomainModels.Exceptions;
using DomainModels.Model;
using DomainModels.Repositories;
using Services;
using Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BussinessLogic.Services
{
    public class PayingItemService : IPayingItemService
    {
        private readonly IRepository<PayingItem> _repository;

        public PayingItemService(IRepository<PayingItem> repository)
        {
            _repository = repository;
        }

        public IEnumerable<PayingItem> GetList()
        {
            try
            {
                return _repository.GetList();
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(PayingItemService)} в методе {nameof(GetList)} при обращении к БД", e);
            }
        }

        public async Task<PayingItem> GetItemAsync(int id)
        {
            try
            {
                return await _repository.GetItemAsync(id);
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(PayingItemService)} в методе {nameof(GetItemAsync)} при обращении к БД", e);
            }

        }

        public async Task<IEnumerable<PayingItem>> GetListAsync()
        {
            try
            {
                return await _repository.GetListAsync();
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(PayingItemService)} в методе {nameof(GetListAsync)} при обращении к БД", e);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                await _repository.DeleteAsync(id);
                await _repository.SaveAsync();
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(PayingItemService)} в методе {nameof(DeleteAsync)} при обращении к БД", e);
            }
        }

        public async Task UpdateAsync(PayingItem item)
        {
            try
            {
                await _repository.UpdateAsync(item);
                await _repository.SaveAsync();
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(PayingItemService)} в методе {nameof(UpdateAsync)} при обращении к БД", e);
            }
        }

        public async Task<PayingItem> CreateAsync(PayingItem item)
        {
            try
            {
                var createdItem = await _repository.CreateAsync(item);
                 await _repository.SaveAsync();

                return createdItem;
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(PayingItemService)} в методе {nameof(CreateAsync)} при обращении к БД", e);
            }
        }

        public IEnumerable<PayingItem> GetListByTypeOfFlow(IWorkingUser user, int typeOfFlow)
        {
            try
            {
                return _repository.GetList(u => u.UserId == user.Id && u.Category.TypeOfFlowID == typeOfFlow);
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(PayingItemService)} в методе {nameof(GetListByTypeOfFlow)} при обращении к БД", e);
            }

        }

        public IEnumerable<PayingItem> GetList(Expression<Func<PayingItem, bool>> predicate)
        {
            try
            {
                return _repository.GetList(predicate);
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(PayingItemService)} в методе {nameof(GetList)} при обращении к БД", e);
            }
        }

        public PayingItem GetItem(int id)
        {
            try
            {
                return _repository.GetItem(id);
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(PayingItemService)} в методе {nameof(GetItem)} при обращении к БД", e);
            }
        }

        public async Task<IEnumerable<PayingItem>> GetListAsync(Expression<Func<PayingItem, bool>> predicate)
        {
            try
            {
                return await _repository.GetListAsync(predicate);
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(PayingItemService)} в методе {nameof(GetListAsync)} при обращении к БД", e);
            }
        }
    }
}