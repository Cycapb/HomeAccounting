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
    public class PayingItemProductService : IPayingItemProductService
    {
        private readonly IRepository<PayingItemProduct> _pItemRepository;

        public PayingItemProductService(IRepository<PayingItemProduct> pItemRepository)
        {
            _pItemRepository = pItemRepository;
        }

        public async Task CreateAsync(PayingItemProduct product)
        {
            try
            {
                await _pItemRepository.CreateAsync(product);
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(PayingItemProductService)} в методе {nameof(CreateAsync)} при обращении к БД", e);
            }
        }

        public async Task<PayingItemProduct> GetItemAsync(int id)
        {
            try
            {
                return await _pItemRepository.GetItemAsync(id);
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(PayingItemProductService)} в методе {nameof(GetItemAsync)} при обращении к БД", e);
            }
        }

        public IEnumerable<PayingItemProduct> GetList()
        {
            try
            {
                return _pItemRepository.GetList();
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(PayingItemProductService)} в методе {nameof(GetList)} при обращении к БД", e);
            }
        }

        public async Task<IEnumerable<PayingItemProduct>> GetListAsync()
        {
            try
            {
                return await _pItemRepository.GetListAsync();
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(PayingItemProductService)} в методе {nameof(GetListAsync)} при обращении к БД", e);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                await _pItemRepository.DeleteAsync(id);
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(PayingItemProductService)} в методе {nameof(DeleteAsync)} при обращении к БД", e);
            }
        }

        public async Task UpdateAsync(PayingItemProduct item)
        {
            try
            {
                await _pItemRepository.UpdateAsync(item);
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(PayingItemProductService)} в методе {nameof(UpdateAsync)} при обращении к БД", e);
            }
        }

        public async Task SaveAsync()
        {
            try
            {
                await _pItemRepository.SaveAsync();
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(PayingItemProductService)} в методе {nameof(SaveAsync)} при обращении к БД", e);
            }
        }

        public IEnumerable<PayingItemProduct> GetList(Expression<Func<PayingItemProduct, bool>> predicate)
        {
            try
            {
                return _pItemRepository.GetList(predicate);
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(PayingItemProductService)} в методе {nameof(GetList)} при обращении к БД", e);
            }
        }

        public PayingItemProduct GetItem(int id)
        {
            try
            {
                return _pItemRepository.GetItem(id);
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(PayingItemProductService)} в методе {nameof(GetItem)} при обращении к БД", e);
            }
        }
    }
}
