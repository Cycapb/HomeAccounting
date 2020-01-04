using DomainModels.Model;
using Services.BaseInterfaces;
namespace Services
{
    /// <summary>
    /// Is used to work with notification mailboxes of the system
    /// </summary>
    public interface IMailboxService : IQueryService<NotificationMailBox>, IQueryServiceAsync<NotificationMailBox>, ICommandServiceAsync<NotificationMailBox>
    {
    }
}
