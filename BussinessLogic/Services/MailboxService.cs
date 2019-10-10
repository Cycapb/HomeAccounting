using System;
using System.Collections.Generic;
using System.Text;
using DomainModels.Repositories;
using System.Threading.Tasks;
using DomainModels.Exceptions;
using DomainModels.Model;
using Services;
using NLog;
using Services.Exceptions;
using System.Linq.Expressions;

namespace BussinessLogic.Services
{
    public class MailboxService : IMailboxService
    {
        private readonly IRepository<NotificationMailBox> _repository;        

        public MailboxService(IRepository<NotificationMailBox> repository)
        {
            _repository = repository;
        }

        public async Task<NotificationMailBox> AddAsync(NotificationMailBox mailbox)
        {
            try
            {
                var box = await _repository.CreateAsync(mailbox);
                await _repository.SaveAsync();
                return box;
            }
            catch (DomainModelsException e)
            {
                var message = CreateMessage(e);                
                throw new ServiceException($"Ошибка в сервисе {nameof(MailboxService)} в методе {nameof(AddAsync)} при обращении к БД", e);
            }
        }        

        public async Task DeleteAsync(int id)
        {
            try
            {
                await _repository.DeleteAsync(id);
                await _repository.SaveAsync();
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(MailboxService)} в методе {nameof(DeleteAsync)} при обращении к БД", e);
            }
        }

        public async Task<NotificationMailBox> GetItemAsync(int id)
        {
            try
            {
                return await _repository.GetItemAsync(id);
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(MailboxService)} в методе {nameof(GetItemAsync)} при обращении к БД", e);
            }
        }

        public async Task<IEnumerable<NotificationMailBox>> GetListAsync()
        {
            try
            {
                return await _repository.GetListAsync();
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(MailboxService)} в методе {nameof(GetListAsync)} при обращении к БД", e);
            }
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
            try
            {
                await _repository.UpdateAsync(item);
                await _repository.SaveAsync();
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(MailboxService)} в методе {nameof(UpdateAsync)} при обращении к БД", e);
            }
        }

        public IEnumerable<NotificationMailBox> GetList()
        {
            try
            {
                return _repository.GetList();
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(MailboxService)} в методе {nameof(GetList)} при обращении к БД", e);
            }
        }

        public IEnumerable<NotificationMailBox> GetList(Expression<Func<NotificationMailBox, bool>> predicate)
        {
            try
            {
                return _repository.GetList(predicate);
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(MailboxService)} в методе {nameof(GetList)} при обращении к БД", e);
            }
        }
    }
}
