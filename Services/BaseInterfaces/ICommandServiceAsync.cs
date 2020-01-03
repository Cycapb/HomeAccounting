using DomainModels.Model;
using System.Threading.Tasks;

namespace Services.BaseInterfaces
{
    public interface ICommandServiceAsync<T> where T : class
    {
        Task DeleteAsync(int id);

        Task UpdateAsync(PayingItem item);

        Task<PayingItem> CreateAsync(PayingItem item);

        Task SaveAsync();
    }
}
