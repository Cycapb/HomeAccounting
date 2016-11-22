using System.Collections.Generic;
using System.Threading.Tasks;
using HomeAccountingSystem_DAL.Model;

namespace Services
{
    public interface IOrderDetailService
    {
        Task<OrderDetail> GetItemAsync(int id);
        Task DeleteAsync(int id);
        Task<IEnumerable<OrderDetail>> GetListAsync();
        Task<OrderDetail> CreateAsync(OrderDetail orderDetail);
    }
}