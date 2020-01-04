using DomainModels.Model;
using Services;
using Services.Exceptions;
using Services.Triggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BussinessLogic.Services
{
    public class PayingItemServiceTriggerDecorator : IPayingItemService
    {
        private readonly IPayingItemService _payingItemService;
        private readonly IServiceTrigger<PayingItem> _serviceTrigger;
        private readonly ICategoryService _categoryService;
        private readonly ITypeOfFlowService _typeOfFlowService;

        public PayingItemServiceTriggerDecorator(
            IPayingItemService payingItemService,
            IServiceTrigger<PayingItem> serviceTrigger,
            ICategoryService categoryService,
            ITypeOfFlowService typeOfFlowService)
        {
            _payingItemService = payingItemService;
            _serviceTrigger = serviceTrigger;
            _categoryService = categoryService;
            _typeOfFlowService = typeOfFlowService;
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
            try
            {
                (var oldPayingItem, var newPayingItem) = await GetNewAndOldItems(item);
                await _payingItemService.UpdateAsync(item);
                await _serviceTrigger.Update(oldPayingItem, newPayingItem);
            }            
            catch (ServiceException e)
            {
                throw new ServiceException($"Ошибка в декораторе сервиса {nameof(PayingItemServiceTriggerDecorator)} в методе {nameof(DeleteAsync)}", e);
            }
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

        private Task<int> GetCategoryIdAsync(IEnumerable<Category> categories, int itemId)
        {
            return Task.Run(() =>
            {
                var categoryId = 0;
                foreach (var category in categories)
                {
                    foreach (var payingItem in category.PayingItems)
                    {
                        if (payingItem.ItemID == itemId)
                        {
                            categoryId = payingItem.CategoryID;
                            break;
                        }
                        if (categoryId != 0)
                        {
                            break;
                        }
                    }
                }
                return categoryId;
            });
        }

        public IEnumerable<PayingItem> GetList(Expression<Func<PayingItem, bool>> predicate)
        {
            return _payingItemService.GetList(predicate);
        }        

        private async Task<(PayingItem OldItem, PayingItem NewItem)> GetNewAndOldItems(PayingItem item)
        {
            var typeOfFlowId = (await _categoryService.GetItemAsync(item.CategoryID)).TypeOfFlowID;
            var categories = (await _typeOfFlowService.GetCategoriesAsync(typeOfFlowId)).Where(x => x.UserId == item.UserId);
            var oldCategoryId = await GetCategoryIdAsync(categories, item.ItemID);
            var oldCategory = await _categoryService.GetItemAsync(oldCategoryId);
            var oldPayingItem = oldCategory.PayingItems.FirstOrDefault(x => x.ItemID == item.ItemID);
            var newPayingItem = new PayingItem()
            {
                Category = oldCategory,
                AccountID = item.AccountID,
                Summ = item.Summ
            };

            return (oldPayingItem, newPayingItem);
        }

        public PayingItem GetItem(int id)
        {
            return _payingItemService.GetItem(id);
        }

        public async Task<IEnumerable<PayingItem>> GetListAsync(Expression<Func<PayingItem, bool>> predicate)
        {
            return await _payingItemService.GetListAsync(predicate);
        }
    }
}