using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeAccountingSystem_DAL.Abstract;
using HomeAccountingSystem_DAL.Model;
using HomeAccountingSystem_DAL.Repositories;
using Services;

namespace BussinessLogic.Services
{
    public class PayingItemService:IPayingItemService
    {
        private readonly IRepository<PayingItem> _repository;

        public PayingItemService(IRepository<PayingItem> repository)
        {
            _repository = repository;
        }

        public IEnumerable<PayingItem> GetList()
        {
            return _repository.GetList();
        }

        public async Task<PayingItem> GetItemAsync(int id)
        {
            return await _repository.GetItemAsync(id);
        }

        public async Task<IEnumerable<PayingItem>> GetListAsync()
        {
            return await _repository.GetListAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
            await _repository.SaveAsync();
        }

        public async Task UpdateAsync(PayingItem item)
        {
            await _repository.UpdateAsync(item);
            await _repository.SaveAsync();
        }

        public async Task CreateAsync(PayingItem item)
        {
            await _repository.CreateAsync(item);
            await _repository.SaveAsync();
        }

        public IEnumerable<PayingItem> GetListByTypeOfFlow(IWorkingUser user, int typeOfFlow)
        {
            return _repository.GetList()
                .Where(u => u.UserId == user.Id && u.Category.TypeOfFlowID == typeOfFlow);
        }
    }
}