using DomainModels.Model;
using Services.BaseInterfaces;
using System;
using System.Threading.Tasks;

namespace Services
{
    public interface IOrderService :
        IQueryServiceAsync<Order>,
        IUpdateDeleteCommandServiceAsync<Order>,
        ICreateCommandServiceAsync<Order>,
        IDisposable
    {
        Task SendByEmailAsync(int orderId, string mailTo);

        Task CloseOrderAsync(int id);
    }
}