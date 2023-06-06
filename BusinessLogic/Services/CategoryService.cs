using DomainModels.Exceptions;
using DomainModels.Model;
using DomainModels.Repositories;
using Services;
using Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BussinnessLogic.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Category> _categoryRepository;
        private bool _disposed;

        public CategoryService(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Category> CreateAsync(Category item)
        {
            try
            {
                item.Active = true;
                var createdItem = _categoryRepository.Create(item);
                await _categoryRepository.SaveAsync();

                return createdItem;
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(CategoryService)} в методе {nameof(CreateAsync)} при обращении к БД", e);
            }
        }

        public Category GetItem(int id)
        {
            try
            {
                return _categoryRepository.GetItem(id);
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(CategoryService)} в методе {nameof(GetItem)} при обращении к БД", e);
            }
        }

        public async Task<Category> GetItemAsync(int id)
        {
            try
            {
                return await _categoryRepository.GetItemAsync(id);
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(CategoryService)} в методе {nameof(GetItemAsync)} при обращении к БД", e);
            }
        }

        public async Task<IEnumerable<Category>> GetListAsync()
        {
            try
            {
                return await _categoryRepository.GetListAsync();
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(CategoryService)} в методе {nameof(GetListAsync)} при обращении к БД", e);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                await _categoryRepository.DeleteAsync(id);
                await _categoryRepository.SaveAsync();
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(CategoryService)} в методе {nameof(DeleteAsync)} при обращении к БД", e);
            }
        }

        public async Task UpdateAsync(Category item)
        {
            try
            {
                _categoryRepository.Update(item);
                await _categoryRepository.SaveAsync();
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(CategoryService)} в методе {nameof(UpdateAsync)} при обращении к БД", e);
            }
        }

        public async Task<bool> HasDependenciesAsync(int id)
        {
            try
            {
                return (await _categoryRepository.GetItemAsync(id)).PayingItems.Any();
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(CategoryService)} в методе {nameof(HasDependenciesAsync)} при обращении к БД", e);
            }
        }

        public async Task<IEnumerable<Category>> GetActiveGategoriesByUserAsync(string userId)
        {
            try
            {
                return (await _categoryRepository.GetListAsync(x => x.Active && x.UserId == userId)).ToList();
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(CategoryService)} в методе {nameof(GetActiveGategoriesByUserAsync)} при обращении к БД", e);
            }
        }

        public IEnumerable<Category> GetList()
        {
            try
            {
                return _categoryRepository.GetList();
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(CategoryService)} в методе {nameof(GetList)} при обращении к БД", e);
            }
        }

        public IEnumerable<Category> GetList(Expression<Func<Category, bool>> predicate)
        {
            try
            {
                return _categoryRepository.GetList(predicate);
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(CategoryService)} в методе {nameof(GetList)} при обращении к БД", e);
            }
        }

        public async Task<IEnumerable<Category>> GetListAsync(Expression<Func<Category, bool>> predicate)
        {
            try
            {
                return await _categoryRepository.GetListAsync(predicate);
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(CategoryService)} в методе {nameof(GetList)} при обращении к БД", e);
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
                    _categoryRepository.Dispose();
                }

                _disposed = true;
            }
        }
    }
}