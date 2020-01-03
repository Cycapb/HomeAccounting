using DomainModels.Model;
using System.Threading.Tasks;

namespace Services
{
    public interface IOrderDetailService
    {
        Task<OrderDetail> GetItemAsync(int id);
        Task DeleteAsync(int id);
        Task<OrderDetail> CreateAsync(OrderDetail orderDetail);
    }
}