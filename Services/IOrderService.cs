using DomainModels.Model;
using Services.BaseInterfaces;
using System;
using System.Threading.Tasks;

namespace Services
{
    public interface IOrderService : IQueryServiceAsync<Order>, ICommandServiceAsync<Order>, IDisposable
    {
        Task SendByEmail(int orderId, string mailTo);

        Task CloseOrder(int id);
    }
}