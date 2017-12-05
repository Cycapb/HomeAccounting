using System.Collections.Generic;
using System.Threading.Tasks;
using DomainModels.Model;
using Services;
using Services.Triggers;

namespace BussinessLogic.Services
{
    public class PayingItemServiceTriggerDecorator:IPayingItemService
    {
        private readonly IPayingItemService _payingItemService;
        private readonly IServiceTrigger<PayingItem> _serviceTrigger;

        public PayingItemServiceTriggerDecorator(IPayingItemService payingItemService, IServiceTrigger<PayingItem> serviceTrigger)
        {
            _payingItemService = payingItemService;
            _serviceTrigger = serviceTrigger;
        }

        public IEnumerable<PayingItem> GetList()
        {
            return _payingItemService.GetList();
        }

        public async Task<PayingItem> GetItemAsync(int id)
        {
            return await _payingItemService.GetItemAsync(id);
        }

        public async Task<IEnumerable<PayingItem>> GetListAsync()
        {
            return await _payingItemService.GetListAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var deletedItem = await _payingItemService.GetItemAsync(id);
            await _payingItemService.DeleteAsync(id);
            await _serviceTrigger.Delete(deletedItem);
        }

        public async Task UpdateAsync(PayingItem item)
        {
            var oldItem = await _payingItemService.GetItemAsync(item.ItemID);
            await _payingItemService.UpdateAsync(item);
            await _serviceTrigger.Update(oldItem, item);
        }

        public async Task<PayingItem> CreateAsync(PayingItem item)
        {
            var insertedItem = await _payingItemService.CreateAsync(item);
            await _serviceTrigger.Insert(insertedItem);
            return insertedItem;
        }

        public IEnumerable<PayingItem> GetListByTypeOfFlow(IWorkingUser user, int typeOfFlow)
        {
            return _payingItemService.GetListByTypeOfFlow(user, typeOfFlow);
        }
    }
}