using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeAccountingSystem_DAL.Model;
using HomeAccountingSystem_DAL.Repositories;
using Services;

namespace BussinessLogic.Services
{
    public class PlanItemService:IPlanItemService
    {
        private readonly IRepository<PlanItem> _pItemRepository;

        public PlanItemService(IRepository<PlanItem> pItemRepository)
        {
            _pItemRepository = pItemRepository;
        }

        public async Task<IEnumerable<PlanItem>> GetListAsync(string userId)
        {
            return (await _pItemRepository.GetListAsync()).Where(x => x.UserId == userId);
        }

        public async Task<PlanItem> GetItemAsync(int id)
        {
            return await _pItemRepository.GetItemAsync(id);
        }

        public async Task CreateAsync(PlanItem planItem)
        {
            await _pItemRepository.CreateAsync(planItem);
        }

        public async Task UpdateAsync(PlanItem planItem)
        {
            await _pItemRepository.UpdateAsync(planItem);
        }

        public async Task SaveAsync()
        {
            await _pItemRepository.SaveAsync();
        }
    }
}