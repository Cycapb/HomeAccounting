using System.Collections.Generic;
using System.Threading.Tasks;
using HomeAccountingSystem_DAL.Model;

namespace Services
{
    public interface IPayingItemProductService
    {
        Task CreateAsync(PaiyngItemProduct product);
        Task<PaiyngItemProduct> GetItemAsync(int id);
        IEnumerable<PaiyngItemProduct> GetList();
        Task<IEnumerable<PaiyngItemProduct>> GetListAsync();
        Task DeleteAsync(int id);
        Task UpdateAsync(PaiyngItemProduct item);
        Task SaveAsync();
    }
}