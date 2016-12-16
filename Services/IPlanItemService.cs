using System.Collections.Generic;
using System.Threading.Tasks;
using HomeAccountingSystem_DAL.Model;

namespace Services
{
    public interface IPlanItemService
    {
        Task<IEnumerable<PlanItem>> GetListAsync(string userId);
        Task<PlanItem> GetItemAsync(int id);
        Task CreateAsync(PlanItem planItem);
        Task UpdateAsync(PlanItem planItem);
        Task SaveAsync();
    }
}