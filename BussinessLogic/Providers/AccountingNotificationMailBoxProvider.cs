using System.Linq;
using DomainModels.Model;
using DomainModels.Repositories;
using Services;

namespace BussinessLogic.Providers
{
    /// <summary>
    /// Служит для получения настроек электронной почты Бухгалтерии
    /// </summary>
    public class AccountingNotificationMailBoxProvider : IMailSettingsProvider
    {
        private readonly IRepository<NotificationMailBox> _repository;

        public AccountingNotificationMailBoxProvider(IRepository<NotificationMailBox> repository)
        {
            _repository = repository;
        }

        private EmailSettings GetMailSettings()
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

        public virtual EmailSettings GetEmailSettings()
        {
            var mailSettings = GetMailSettings();
            return mailSettings;
        }
    }
}
