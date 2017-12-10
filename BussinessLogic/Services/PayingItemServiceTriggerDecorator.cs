using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModels.Model;
using Services;
using Services.Exceptions;
using Services.Triggers;

namespace BussinessLogic.Services
{
    public class PayingItemServiceTriggerDecorator:IPayingItemService
    {
        private readonly IPayingItemService _payingItemService;
        private readonly IServiceTrigger<PayingItem> _serviceTrigger;
        private readonly ICategoryService _categoryService;

        public PayingItemServiceTriggerDecorator(IPayingItemService payingItemService, IServiceTrigger<PayingItem> serviceTrigger, ICategoryService categoryService)
        {
            _payingItemService = payingItemService;
            _serviceTrigger = serviceTrigger;
            _categoryService = categoryService;
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
            try
            {
                var deletedItem = await _payingItemService.GetItemAsync(id);
                await _payingItemService.DeleteAsync(id);
                await _serviceTrigger.Delete(deletedItem);
            }
            catch (ServiceException e)
            {
                throw new ServiceException($"Ошибка в декораторе сервиса {nameof(PayingItemServiceTriggerDecorator)} в методе {nameof(DeleteAsync)}", e);
            }
        }

        public async Task UpdateAsync(PayingItem item)
        {
            var oldCategory = await _categoryService.GetItemAsync(item.CategoryID);
            var oldPayingItem = oldCategory.PayingItem.FirstOrDefault(x => x.ItemID == item.ItemID);
            var newItem = new PayingItem()
            {
                Category = oldCategory,
                AccountID = item.AccountID,
                Summ = item.Summ
            };
            await _payingItemService.UpdateAsync(item);
            await _serviceTrigger.Update(oldPayingItem, newItem);
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