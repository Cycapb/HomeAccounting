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
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _productRepository;

        public ProductService(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task CreateAsync(Product product)
        {
            try
            {
                await _productRepository.CreateAsync(product);
                await _productRepository.SaveAsync();
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
                var dependencies = (await _productRepository.GetItemAsync(id)).PaiyngItemProduct.Any();
                if (dependencies)
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
                await _productRepository.UpdateAsync(item);
                await _productRepository.SaveAsync();
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(ProductService)} в методе {nameof(UpdateAsync)} при обращении к БД", e);
            }
        }

        public async Task SaveAsync()
        {
            try
            {
                await _productRepository.SaveAsync();
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(ProductService)} в методе {nameof(SaveAsync)} при обращении к БД", e);
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
    }
}