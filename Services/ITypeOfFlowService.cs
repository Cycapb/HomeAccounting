using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DomainModels.Model;

namespace Services
{
    public interface ITypeOfFlowService : IDisposable
    {
        IEnumerable<TypeOfFlow> GetList();
        Task<IEnumerable<TypeOfFlow>> GetListAsync();
        Task<IEnumerable<Category>> GetCategoriesAsync(int typeOfFlowId);
    }
}