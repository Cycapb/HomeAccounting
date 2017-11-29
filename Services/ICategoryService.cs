using System.Collections.Generic;
using System.Threading.Tasks;
using DomainModels.Model;

namespace Services
{
    public interface ICategoryService
    {
        Task CreateAsync(Category item);
        Category GetItem(int id);
        Task<Category> GetItemAsync(int id);
        Task<IEnumerable<Category>> GetListAsync();
        IEnumerable<Category> GetList();
        Task DeleteAsync(int id);
        Task UpdateAsync(Category item);
        Task SaveAsync();
        Task<IEnumerable<Product>> GetProducts(int id);
        Task<bool> HasDependencies(int id);
        Task<IEnumerable<Category>> GetActiveGategoriesByUser(string userId);
    }
}