using System.Collections.Generic;
using System.Threading.Tasks;
using HomeAccountingSystem_DAL.Model;

namespace Services
{
    public interface IProductService
    {
        Task CreateAsync(Product product);
        Task<Product> GetItemAsync(int id);
        IEnumerable<Product> GetList();
        Task<IEnumerable<Product>> GetListAsync();
        Task DeleteAsync(int id);
        Task UpdateAsync(Product item);
        Task SaveAsync();
    }
}