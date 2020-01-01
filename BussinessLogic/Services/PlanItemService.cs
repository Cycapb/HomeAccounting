using DomainModels.Exceptions;
using DomainModels.Model;
using DomainModels.Repositories;
using Services;
using Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BussinessLogic.Services
{
    public class PlanItemService : IPlanItemService
    {
        private readonly IRepository<PlanItem> _pItemRepository;

        public PlanItemService(IRepository<PlanItem> pItemRepository)
        {
            _pItemRepository = pItemRepository;
        }

        public async Task<PlanItem> GetItemAsync(int id)
        {
            try
            {
                return await _pItemRepository.GetItemAsync(id);
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(PlanItemService)} в методе {nameof(GetItemAsync)} при обращении к БД", e);
            }
        }

        public async Task CreateAsync(PlanItem planItem)
        {
            try
            {
                await _pItemRepository.CreateAsync(planItem);
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(PlanItemService)} в методе {nameof(CreateAsync)} при обращении к БД", e);
            }
        }

        public async Task UpdateAsync(PlanItem planItem)
        {
            try
            {
                await _pItemRepository.UpdateAsync(planItem);
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(PlanItemService)} в методе {nameof(UpdateAsync)} при обращении к БД", e);
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
                throw new ServiceException($"Ошибка в сервисе {nameof(PlanItemService)} в методе {nameof(SaveAsync)} при обращении к БД", e);
            }
        }

        public async Task<IEnumerable<PlanItem>> GetListAsync()
        {
            try
            {
                return await _pItemRepository.GetListAsync();
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(PlanItemService)} в методе {nameof(GetListAsync)} при обращении к БД", e);
            }
        }

        public async Task<IEnumerable<PlanItem>> GetListAsync(Expression<Func<PlanItem, bool>> predicate)
        {
            try
            {
                return await _pItemRepository.GetListAsync(predicate);
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(PlanItemService)} в методе {nameof(GetListAsync)} при обращении к БД", e);
            }
        }
    }
}