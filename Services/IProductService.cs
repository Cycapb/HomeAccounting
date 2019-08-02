using DomainModels.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public interface IProductService : IService<Product>
    {
        Task CreateAsync(Product product);
        Task<Product> GetItemAsync(int id);        
        Task<IEnumerable<Product>> GetListAsync();
        Task DeleteAsync(int id);
        Task UpdateAsync(Product item);
        Task SaveAsync();
    }
}