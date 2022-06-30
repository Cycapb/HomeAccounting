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
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _productRepository;
        private bool _disposed;

        public ProductService(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Product> CreateAsync(Product product)
        {
            try
            {
                var createdItem = _productRepository.Create(product);
                await _productRepository.SaveAsync();

                return createdItem;
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(ProductService)} в методе {nameof(CreateAsync)} при обращении к БД", e);
            }
        }

        public async Task<Product> GetItemAsync(int id)
        {
            try
            {
                return await _productRepository.GetItemAsync(id);
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(ProductService)} в методе {nameof(GetItemAsync)} при обращении к БД", e);
            }
        }

        public IEnumerable<Product> GetList()
        {
            try
            {
                return _productRepository.GetList();
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(ProductService)} в методе {nameof(GetList)} при обращении к БД", e);
            }
        }

        public async Task<IEnumerable<Product>> GetListAsync()
        {
            try
            {
                return await _productRepository.GetListAsync();
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(ProductService)} в методе {nameof(GetListAsync)} при обращении к БД", e);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var hasDependencies = (await _productRepository.GetItemAsync(id)).PayingItemProducts.Any();
                if (hasDependencies)
                {
                    return;
                }
                await _productRepository.DeleteAsync(id);
                await _productRepository.SaveAsync();
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(ProductService)} в методе {nameof(DeleteAsync)} при обращении к БД", e);
            }
        }

        public async Task UpdateAsync(Product item)
        {
            try
            {
                _productRepository.Update(item);
                await _productRepository.SaveAsync();
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(ProductService)} в методе {nameof(UpdateAsync)} при обращении к БД", e);
            }
        }

        public IEnumerable<Product> GetList(Expression<Func<Product, bool>> predicate)
        {
            try
            {
                return _productRepository.GetList(predicate);
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(ProductService)} в методе {nameof(GetList)} при обращении к БД", e);
            }
        }

        public Product GetItem(int id)
        {
            try
            {
                return _productRepository.GetItem(id);
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(ProductService)} в методе {nameof(GetItem)} при обращении к БД", e);
            }
        }

        public async Task<IEnumerable<Product>> GetListAsync(Expression<Func<Product, bool>> predicate)
        {
            try
            {
                return await _productRepository.GetListAsync(predicate);
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(ProductService)} в методе {nameof(GetListAsync)} при обращении к БД", e);
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
                    _productRepository.Dispose();
                }

                _disposed = true;
            }
        }
    }
}