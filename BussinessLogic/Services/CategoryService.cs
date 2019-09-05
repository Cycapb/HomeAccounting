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

namespace BussinessLogic.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Category> _categoryRepository;

        public CategoryService(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task CreateAsync(Category item)
        {
            try
            {
                item.Active = true;
                await _categoryRepository.CreateAsync(item);
                await _categoryRepository.SaveAsync();
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
                await _categoryRepository.UpdateAsync(item);
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(CategoryService)} в методе {nameof(UpdateAsync)} при обращении к БД", e);
            }
        }

        public async Task SaveAsync()
        {
            try
            {
                await _categoryRepository.SaveAsync();
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(CategoryService)} в методе {nameof(SaveAsync)} при обращении к БД", e);
            }
        }

        public async Task<IEnumerable<Product>> GetProducts(int id)
        {
            try
            {
                return (await _categoryRepository.GetItemAsync(id)).Products;
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(CategoryService)} в методе {nameof(GetProducts)} при обращении к БД", e);
            }
        }

        public async Task<bool> HasDependencies(int id)
        {
            try
            {
                return (await _categoryRepository.GetItemAsync(id)).PayingItems.Any();
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(CategoryService)} в методе {nameof(HasDependencies)} при обращении к БД", e);
            }
        }

        public async Task<IEnumerable<Category>> GetActiveGategoriesByUser(string userId)
        {
            try
            {
                return (await _categoryRepository.GetListAsync())?.Where(x => x.Active && x.UserId == userId);
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(CategoryService)} в методе {nameof(GetActiveGategoriesByUser)} при обращении к БД", e);
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
    }
}