using System.Collections.Generic;
using System.Threading.Tasks;
using HomeAccountingSystem_DAL.Model;

namespace Services
{
    public interface ITypeOfFlowService
    {
        IEnumerable<TypeOfFlow> GetList();
        IEnumerable<Category> GetCategories(int typeOfFlowId);
        Task<IEnumerable<TypeOfFlow>> GetListAsync();
        Task<IEnumerable<Category>> GetCategoriesAsync(int typeOfFlowId);
    }
}