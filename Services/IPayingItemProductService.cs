using DomainModels.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public interface IPayingItemProductService : IQueryService<PayingItemProduct>
    {
        Task CreateAsync(PayingItemProduct product);
        Task<PayingItemProduct> GetItemAsync(int id);
        Task<IEnumerable<PayingItemProduct>> GetListAsync();
        Task DeleteAsync(int id);
        Task UpdateAsync(PayingItemProduct item);
        Task SaveAsync();
    }
}