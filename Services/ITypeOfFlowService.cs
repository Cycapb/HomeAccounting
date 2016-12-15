using System.Collections.Generic;
using HomeAccountingSystem_DAL.Model;

namespace Services
{
    public interface ITypeOfFlowService
    {
        IEnumerable<TypeOfFlow> GetList();
        IEnumerable<Category> GetCategories(int typeOfFlowId);
    }
}