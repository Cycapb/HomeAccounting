using DomainModels.Model;
using System.Threading.Tasks;

namespace Services
{
    public interface IProductService : IQueryService<Product>, IQueryServiceAsync<Product>
    {
        Task CreateAsync(Product product);

        Task DeleteAsync(int id);

        Task UpdateAsync(Product item);

        Task SaveAsync();
    }
}