using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainModels.Model;
using Services;

namespace BussinessLogic.Services
{
    public class MailboxService : IMailboxService
    {
        public Task<NotificationMailBox> AddAsync(NotificationMailBox mailbox)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task GetItemAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<NotificationMailBox>> GetListAsync()
        {
            throw new NotImplementedException();
        }

        public void SaveAsync()
        {
            throw new NotImplementedException();
        }
    }
}
