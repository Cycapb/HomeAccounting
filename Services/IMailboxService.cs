using DomainModels.Model;
using System.Threading.Tasks;
namespace Services
{
    /// <summary>
    /// Служит для управления почтовыми ящиками системы уведомлений
    /// </summary>
    public interface IMailboxService : IService<NotificationMailBox>, IServiceAsync<NotificationMailBox>
    {
        Task<NotificationMailBox> AddAsync(NotificationMailBox mailbox);
        Task<NotificationMailBox> GetItemAsync(int id);
        Task DeleteAsync(int id);
        Task UpdateAsync(NotificationMailBox item);        
    }
}
