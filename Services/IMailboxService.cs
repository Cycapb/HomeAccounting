using DomainModels.Model;
using System.Threading.Tasks;
using System.Collections.Generic;
namespace Services
{
    /// <summary>
    /// Служит для управления почтовыми ящиками системы уведомлений
    /// </summary>
    public interface IMailboxService
    {
        Task<NotificationMailBox> AddAsync(NotificationMailBox mailbox);
        Task<NotificationMailBox> GetItemAsync(int id);
        Task<IEnumerable<NotificationMailBox>> GetListAsync();
        Task DeleteAsync(int id);
        Task SaveAsync();
    }
}
