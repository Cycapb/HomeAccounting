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
        Task SendByEmail(int orderId, string mailTo);

        Task CloseOrder(int id);
    }
}