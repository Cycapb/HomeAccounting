using DomainModels.Model;
using System;
using System.Threading.Tasks;

namespace Services
{
    public interface IOrderService : IQueryServiceAsync<Order>
    {
        Task<Order> CreateOrderAsync(DateTime orderDate, string userId);
        Task DeleteAsync(int id);
        Task UpdateAsync(Order order);
        Task SendByEmail(int orderId, string mailTo);
        Task CloseOrder(int id);
    }
}