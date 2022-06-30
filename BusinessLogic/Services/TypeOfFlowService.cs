using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DomainModels.Exceptions;
using DomainModels.Model;
using DomainModels.Repositories;
using Services;
using Services.Exceptions;

namespace BussinnessLogic.Services
{
    public class TypeOfFlowService:ITypeOfFlowService
    {
        private readonly IRepository<TypeOfFlow> _tofRepository;
        private bool _disposed;

        public TypeOfFlowService(IRepository<TypeOfFlow> tofRepository)
        {
            _tofRepository = tofRepository;
        }

        public IEnumerable<TypeOfFlow> GetList()
        {
            try
            {
                return _tofRepository.GetList();
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(TypeOfFlowService)} в методе {nameof(GetList)} при обращении к БД", e);
            }
        }

        public async Task<IEnumerable<TypeOfFlow>> GetListAsync()
        {
            try
            {
                return await _tofRepository.GetListAsync();
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(TypeOfFlowService)} в методе {nameof(GetListAsync)} при обращении к БД", e);
            }
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync(int typeOfFlowId)
        {
            try
            {
                return (await _tofRepository.GetItemAsync(typeOfFlowId)).Categories;
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(TypeOfFlowService)} в методе {nameof(GetCategoriesAsync)} при обращении к БД", e);
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
                    _tofRepository.Dispose();
                }

                _disposed = true;
            }
        }
    }
}