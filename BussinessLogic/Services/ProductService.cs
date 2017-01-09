using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeAccountingSystem_DAL.Model;
using HomeAccountingSystem_DAL.Repositories;
using Services;

namespace BussinessLogic.Services
{
    public class ProductService:IProductService
    {
        private readonly IRepository<Product> _productRepository;

        public ProductService(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task CreateAsync(Product product)
        {
            await _productRepository.CreateAsync(product);
            await _productRepository.SaveAsync();
        }

        public async Task<Product> GetItemAsync(int id)
        {
            return await _productRepository.GetItemAsync(id);
        }

        public IEnumerable<Product> GetList()
        {
            return _productRepository.GetList();
        }

        public async Task<IEnumerable<Product>> GetListAsync()
        {
            return await _productRepository.GetListAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var dependencies = (await _productRepository.GetItemAsync(id)).PaiyngItemProduct.Any();
            if (dependencies)
            {
                return;
            }
            await _productRepository.DeleteAsync(id);
            await _productRepository.SaveAsync();
        }

        public async Task UpdateAsync(Product item)
        {
            await _productRepository.UpdateAsync(item);
            await _productRepository.SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _productRepository.SaveAsync();
        }
    }
}