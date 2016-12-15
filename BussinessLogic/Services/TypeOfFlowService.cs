using System.Collections.Generic;
using System.Threading.Tasks;
using HomeAccountingSystem_DAL.Model;
using HomeAccountingSystem_DAL.Repositories;
using Services;

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
            return _tofRepository.GetList();
        }

        public IEnumerable<Category> GetCategories(int typeOfFlowId)
        {
            return _tofRepository.GetItem(typeOfFlowId).Category;
        }

        public async Task<IEnumerable<TypeOfFlow>> GetListAsync()
        {
            return await _tofRepository.GetListAsync();
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync(int typeOfFlowId)
        {
            return (await _tofRepository.GetItemAsync(typeOfFlowId)).Category;
        }
    }
}