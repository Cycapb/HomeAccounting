using DomainModels.Model;
using Services.BaseInterfaces;
using System;

namespace Services
{
    /// <summary>
    /// Is used to work with notification mailboxes of the system
    /// </summary>
    public interface IMailboxService :
        IQueryService<NotificationMailBox>,
        IQueryServiceAsync<NotificationMailBox>,
        IUpdateDeleteCommandServiceAsync<NotificationMailBox>,
        ICreateCommandServiceAsync<NotificationMailBox>,
        IDisposable
    {
    }
}
