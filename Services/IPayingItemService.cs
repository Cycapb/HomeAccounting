using System.Collections.Generic;
using System.Threading.Tasks;
using DomainModels.Model;

namespace Services
{
    public interface IPayingItemService
    {
        IEnumerable<PayingItem> GetList();
        Task<PayingItem> GetItemAsync(int id);
        Task<IEnumerable<PayingItem>> GetListAsync();
        Task DeleteAsync(int id);
        Task UpdateAsync(PayingItem item);
        Task CreateAsync(PayingItem item);
        IEnumerable<PayingItem> GetListByTypeOfFlow(IWorkingUser user, int typeOfFlow);
    }
}