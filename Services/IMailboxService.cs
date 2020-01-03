﻿using DomainModels.Model;
using Services.BaseInterfaces;
using System.Threading.Tasks;
namespace Services
{
    /// <summary>
    /// Is used to work with notification mailboxes of the system
    /// </summary>
    public interface IMailboxService : IQueryService<NotificationMailBox>, IQueryServiceAsync<NotificationMailBox>
    {
        Task<NotificationMailBox> AddAsync(NotificationMailBox mailbox);
        Task DeleteAsync(int id);
        Task UpdateAsync(NotificationMailBox item);        
    }
}
