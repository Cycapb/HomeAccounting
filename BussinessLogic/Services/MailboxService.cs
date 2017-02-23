using System;
using System.Collections.Generic;
using System.Text;
using DomainModels.Repositories;
using System.Threading.Tasks;
using DomainModels.Model;
using Services;
using NLog;

namespace BussinessLogic.Services
{
    public class MailboxService : IMailboxService
    {
        private readonly IRepository<NotificationMailBox> _repository;
        private static readonly Logger LogManager = NLog.LogManager.GetCurrentClassLogger();

        public MailboxService(IRepository<NotificationMailBox> repository)
        {
            _repository = repository;
        }

        public async Task<NotificationMailBox> AddAsync(NotificationMailBox mailbox)
        {
            NotificationMailBox box = null;
            try
            {
                box = await _repository.CreateAsync(mailbox);
                await _repository.SaveAsync();                
            }
            catch (Exception ex)
            {
                var message = CreateMessage(ex);
                LogManager.Error(message);
            }
            return box;
        }        

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
            await _repository.SaveAsync();
        }

        public async Task<NotificationMailBox> GetItemAsync(int id)
        {
            return await _repository.GetItemAsync(id);
        }

        public async Task<IEnumerable<NotificationMailBox>> GetListAsync()
        {
            return await  _repository.GetListAsync();
        }

        private string CreateMessage(Exception ex)
        {
            var errorMessage = new StringBuilder();
            errorMessage.AppendLine("\r\n");
            errorMessage.AppendLine($"Ошибка: {ex.Message}");
            errorMessage.AppendLine($"Трассировка стэка: {ex.StackTrace}");
            return errorMessage.ToString();
        }

        public async Task UpdateAsync(NotificationMailBox item)
        {
            await _repository.UpdateAsync(item);
            await _repository.SaveAsync();
        }
    }
}
