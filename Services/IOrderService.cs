using DomainModels.Model;
using Services.BaseInterfaces;
using System.Threading.Tasks;

namespace Services
{
    public interface IOrderService : IQueryServiceAsync<Order>, ICommandServiceAsync<Order>
    {
        Task SendByEmail(int orderId, string mailTo);

        Task CloseOrder(int id);
    }
}