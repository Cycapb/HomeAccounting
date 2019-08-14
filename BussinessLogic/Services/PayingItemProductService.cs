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
        private readonly IRepository<PaiyngItemProduct> _pItemRepository;

        public PayingItemProductService(IRepository<PaiyngItemProduct> pItemRepository)
        {
            _pItemRepository = pItemRepository;
        }

        public async Task CreateAsync(PaiyngItemProduct product)
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

        public async Task<PaiyngItemProduct> GetItemAsync(int id)
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

        public IEnumerable<PaiyngItemProduct> GetList()
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

        public async Task<IEnumerable<PaiyngItemProduct>> GetListAsync()
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

        public async Task UpdateAsync(PaiyngItemProduct item)
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

        public IEnumerable<PaiyngItemProduct> GetList(Expression<Func<PaiyngItemProduct, bool>> predicate)
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
    }
}
