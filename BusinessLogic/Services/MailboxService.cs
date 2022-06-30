using DomainModels.Exceptions;
using DomainModels.Model;
using DomainModels.Repositories;
using Services;
using Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BussinnessLogic.Services
{
    public class MailboxService : IMailboxService
    {
        private readonly IRepository<NotificationMailBox> _repository;
        private bool _disposed;

        public MailboxService(IRepository<NotificationMailBox> repository)
        {
            _repository = repository;
        }

        public async Task<NotificationMailBox> CreateAsync(NotificationMailBox mailbox)
        {
            try
            {
                var box = _repository.Create(mailbox);
                await _repository.SaveAsync();

                return box;
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(MailboxService)} в методе {nameof(CreateAsync)} при обращении к БД", e);
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

        public async Task UpdateAsync(NotificationMailBox item)
        {
            try
            {
                _repository.Update(item);
                await _repository.SaveAsync();
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(MailboxService)} в методе {nameof(UpdateAsync)} при обращении к БД", e);
            }
        }

        public async Task<IEnumerable<NotificationMailBox>> GetListAsync(Expression<Func<NotificationMailBox, bool>> predicate)
        {
            try
            {
                return await _repository.GetListAsync(predicate);
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(MailboxService)} в методе {nameof(GetListAsync)} при обращении к БД", e);
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

        public NotificationMailBox GetItem(int id)
        {
            try
            {
                return _repository.GetItem(id);
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(MailboxService)} в методе {nameof(GetItem)} при обращении к БД", e);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _repository.Dispose();
                }

                _disposed = true;
            }
        }
    }
}