﻿using System;
using System.Collections.Generic;
using System.Linq;
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
        private IRepository<NotificationMailBox> _repository;
        private static readonly Logger _logManager = LogManager.GetCurrentClassLogger();

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
                _logManager.Error(message);
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

        public Task<IEnumerable<NotificationMailBox>> GetListAsync()
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync()
        {
            throw new NotImplementedException();
        }

        private string CreateMessage(Exception ex)
        {
            var errorMessage = new StringBuilder();
            errorMessage.AppendLine("\r\n");
            errorMessage.AppendLine($"Ошибка: {ex.Message}");
            errorMessage.AppendLine($"Трассировка стэка: {ex.StackTrace}");
            return errorMessage.ToString();
        }
    }
}