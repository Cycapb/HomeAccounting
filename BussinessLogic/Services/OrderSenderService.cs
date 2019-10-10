using DomainModels.Model;
using NLog;
using Services;
using Services.Exceptions;
using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogic.Services
{
    /// <summary>
    /// Служит для отправки Списка покупок по электронной почте
    /// Наследует от IEmailSender
    /// </summary>    
    public class OrderSenderService : IEmailSender
    {
        private readonly IMailSettingsProvider _mailSettingsProvider;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public string MailTo { get; private set; }

        public OrderSenderService(IMailSettingsProvider mailSettingsProvider)
        {
            _mailSettingsProvider = mailSettingsProvider;
        }

        public async Task SendAsync(string message, string mailTo)
        {
            var mailSettings = _mailSettingsProvider.GetEmailSettings();
            if (mailSettings != null)
            {
                mailSettings.MailTo = mailTo;
                var smtpClient = CreateSmtpClient(mailSettings);

                try
                {
                    var mailMessage = new MailMessage(mailSettings.MailFrom, mailSettings.MailTo, "Список покупок", message);
                    await smtpClient.SendMailAsync(mailMessage);
                }
                catch (Exception ex)
                {
                    throw new SendEmailException($"Возникла ошибка в сервисе {nameof(OrderSenderService)} в методе {nameof(SendAsync)} при отправке почты", ex);
                }
            }
            else
            {
                var errorMessage = new StringBuilder();
                errorMessage.AppendLine("\r\n");
                errorMessage.AppendLine($"Ошибка: Невозможно получить учетные данные почтового ящика для отправки списка покупок!");
                Logger.Error(errorMessage.ToString);
            }

        }

        private SmtpClient CreateSmtpClient(EmailSettings mailSettings)
        {
            return new SmtpClient()
            {
                Host = mailSettings.ServerName,
                Port = mailSettings.ServerPort,
                EnableSsl = mailSettings.UseSsl,
                Credentials = new NetworkCredential(mailSettings.UserName, mailSettings.Password)
            };
        }
    }
}