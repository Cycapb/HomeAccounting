using DomainModels.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public interface IPayingItemProductService : IService<PaiyngItemProduct>
    {
        Task CreateAsync(PaiyngItemProduct product);
        Task<PaiyngItemProduct> GetItemAsync(int id);
        Task<IEnumerable<PaiyngItemProduct>> GetListAsync();
        Task DeleteAsync(int id);
        Task UpdateAsync(PaiyngItemProduct item);
        Task SaveAsync();
    }
}