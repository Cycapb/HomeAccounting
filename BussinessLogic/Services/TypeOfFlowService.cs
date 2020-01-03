using System.Collections.Generic;
using System.Threading.Tasks;
using DomainModels.Exceptions;
using DomainModels.Model;
using DomainModels.Repositories;
using Services;
using Services.Exceptions;

namespace BussinessLogic.Services
{
    public class TypeOfFlowService:ITypeOfFlowService
    {
        private readonly IRepository<TypeOfFlow> _tofRepository;

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

        public IEnumerable<Category> GetCategories(int typeOfFlowId)
        {
            try
            {
                return _tofRepository.GetItem(typeOfFlowId).Categories;
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(TypeOfFlowService)} в методе {nameof(GetCategories)} при обращении к БД", e);
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
    }
}