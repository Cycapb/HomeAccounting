using DomainModels.Exceptions;
using DomainModels.Model;
using DomainModels.Repositories;
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
    public class PayingItemService : IPayingItemService
    {
        private bool _disposed;
        private readonly IRepository<PayingItem> _repository;
        private readonly IServiceTrigger<PayingItem> _serviceTrigger;
        private readonly ICategoryService _categoryService;
        private readonly ITypeOfFlowService _typeOfFlowService;

        public PayingItemService(
            IRepository<PayingItem> repository,
            IServiceTrigger<PayingItem> serviceTrigger,
            ICategoryService categoryService,
            ITypeOfFlowService typeOfFlowService)
        {
            _repository = repository;
            _serviceTrigger = serviceTrigger;
            _categoryService = categoryService;
            _typeOfFlowService = typeOfFlowService;
        }

        public IEnumerable<PayingItem> GetList()
        {
            try
            {
                return _repository.GetList();
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(PayingItemService)} в методе {nameof(GetList)} при обращении к БД", e);
            }
        }

        public async Task<PayingItem> GetItemAsync(int id)
        {
            try
            {
                return await _repository.GetItemAsync(id);
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(PayingItemService)} в методе {nameof(GetItemAsync)} при обращении к БД", e);
            }

        }

        public async Task<IEnumerable<PayingItem>> GetListAsync()
        {
            try
            {
                return await _repository.GetListAsync();
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(PayingItemService)} в методе {nameof(GetListAsync)} при обращении к БД", e);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var itemToDelete = await _repository.GetItemAsync(id);
                await _repository.DeleteAsync(id);
                await _repository.SaveAsync();
                await _serviceTrigger.Delete(itemToDelete);
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(PayingItemService)} в методе {nameof(DeleteAsync)} при обращении к БД", e);
            }
            catch (ServiceException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(PayingItemService)} в методе {nameof(DeleteAsync)} при вызове сервиса", e);
            }
        }

        public async Task UpdateAsync(PayingItem item)
        {
            try
            {
                (var oldPayingItem, var newPayingItem) = await GetNewAndOldItems(item);
                _repository.Update(item);
                await _repository.SaveAsync();
                await _serviceTrigger.Update(oldPayingItem, newPayingItem);
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(PayingItemService)} в методе {nameof(UpdateAsync)} при обращении к БД", e);
            }
        }

        public async Task<PayingItem> CreateAsync(PayingItem item)
        {
            try
            {
                var createdItem = _repository.Create(item);
                await _repository.SaveAsync();
                await _serviceTrigger.Insert(createdItem);

                return createdItem;
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(PayingItemService)} в методе {nameof(CreateAsync)} при обращении к БД", e);
            }
        }

        public IEnumerable<PayingItem> GetListByTypeOfFlow(IWorkingUser user, int typeOfFlow)
        {
            try
            {
                return _repository.GetList(u => u.UserId == user.Id && u.Category.TypeOfFlowID == typeOfFlow);
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(PayingItemService)} в методе {nameof(GetListByTypeOfFlow)} при обращении к БД", e);
            }

        }

        public IEnumerable<PayingItem> GetList(Expression<Func<PayingItem, bool>> predicate)
        {
            try
            {
                return _repository.GetList(predicate);
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(PayingItemService)} в методе {nameof(GetList)} при обращении к БД", e);
            }
        }

        public PayingItem GetItem(int id)
        {
            try
            {
                return _repository.GetItem(id);
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(PayingItemService)} в методе {nameof(GetItem)} при обращении к БД", e);
            }
        }

        public async Task<IEnumerable<PayingItem>> GetListAsync(Expression<Func<PayingItem, bool>> predicate)
        {
            try
            {
                return await _repository.GetListAsync(predicate);
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(PayingItemService)} в методе {nameof(GetListAsync)} при обращении к БД", e);
            }
        }

        public async Task<IEnumerable<PayingItem>> GetListByTypeOfFlowAsync(string userId, int typeOfFlow)
        {
            try
            {
                return await _repository.GetListAsync(u => u.UserId == userId && u.Category.TypeOfFlowID == typeOfFlow);
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(PayingItemService)} в методе {nameof(GetListByTypeOfFlowAsync)} при обращении к БД", e);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _repository.Dispose();
                    _categoryService.Dispose();
                    _typeOfFlowService.Dispose();
                }

                _disposed = true;
            }
        }

        private async Task<(PayingItem OldItem, PayingItem NewItem)> GetNewAndOldItems(PayingItem item)
        {
            var typeOfFlowId = (await _categoryService.GetItemAsync(item.CategoryID)).TypeOfFlowID;
            var categories = (await _typeOfFlowService.GetCategoriesAsync(typeOfFlowId)).Where(x => x.UserId == item.UserId);
            var oldCategoryId = GetCategoryId(categories, item.ItemID);
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

        private int GetCategoryId(IEnumerable<Category> categories, int payingItemId)
        {
            var categoryId = 0;
            foreach (var category in categories)
            {
                foreach (var payingItem in category.PayingItems)
                {
                    if (payingItem.ItemID == payingItemId)
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
        }
    }
}