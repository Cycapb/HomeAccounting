using System.Collections.Generic;
using System.Threading.Tasks;
using DomainModels.Model;

namespace Services
{
    public interface IPayingItemService : IService<PayingItem>
    {        
        Task<PayingItem> GetItemAsync(int id);
        Task<IEnumerable<PayingItem>> GetListAsync();
        Task DeleteAsync(int id);
        Task UpdateAsync(PayingItem item);
        Task<PayingItem> CreateAsync(PayingItem item);
        IEnumerable<PayingItem> GetListByTypeOfFlow(IWorkingUser user, int typeOfFlow);
        Task SaveAsync();
    }
}