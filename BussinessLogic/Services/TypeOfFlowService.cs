using System.Collections.Generic;
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
    }
}