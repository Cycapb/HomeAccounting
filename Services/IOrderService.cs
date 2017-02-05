using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DomainModels.Model;

namespace Services
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(DateTime orderDate, string userId);
        Task DeleteAsync(int id);
        Task UpdateAsync(Order order);
        Task<IEnumerable<Order>> GetList(string userId);
        void SendByEmail(int orderId);
        Task CloseOrder(int id);
        Task<Order> GetOrderAsync(int id);
    }
}