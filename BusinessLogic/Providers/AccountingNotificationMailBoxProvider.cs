using System;
using System.Linq;
using DomainModels.Model;
using DomainModels.Repositories;
using Providers;

namespace BusinessLogic.Providers
{
    /// <summary>
    /// Служит для получения настроек электронной почты Бухгалтерии
    /// </summary>
    public class AccountingNotificationMailBoxProvider : IMailSettingsProvider
    {
        private readonly IRepository<NotificationMailBox> _repository;
        private bool _disposed;

        public AccountingNotificationMailBoxProvider(IRepository<NotificationMailBox> repository)
        {
            _repository = repository;
        }

        public virtual EmailSettings GetEmailSettings()
        {
            var mailSettings = new EmailSettings();
            var mailBox = _repository.GetList(x => x.MailBoxName == "Accounting").SingleOrDefault();

            if (mailBox != null)
            {
                mailSettings.ServerName = mailBox.Server;
                mailSettings.ServerPort = mailBox.Port;
                mailSettings.MailFrom = mailBox.MailFrom;
                mailSettings.UserName = mailBox.UserName;
                mailSettings.Password = mailBox.Password;
                mailSettings.UseSsl = mailBox.UseSsl;
                return mailSettings;
            }
            else
            {
                return null;
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
