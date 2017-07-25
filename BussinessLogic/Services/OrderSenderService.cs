using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using DomainModels.Model;
using Services;
using NLog;

namespace BussinessLogic.Services
{
    /// <summary>
    /// Служит для отправки Списка покупок по электронной почте
    /// Наследует от IEmailSender
    /// </summary>    
    public class OrderSenderService:IEmailSender
    {
        private readonly IMailSettingsProvider _mailSettingsProvider;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public string MailTo { get; set; }

        public OrderSenderService(IMailSettingsProvider mailSettingsProvider)
        {
            _mailSettingsProvider = mailSettingsProvider;
        }

        public async void Send(string message)
        {
            var mailSettings = _mailSettingsProvider.GetEmailSettings();
            if (mailSettings != null)
            {
                mailSettings.MailTo = MailTo;
                var smtpClient = CreateSmtpClient(mailSettings);

                try
                {
                    var mailMessage = new MailMessage(mailSettings.MailFrom, mailSettings.MailTo, "Список покупок", message);
                    await smtpClient.SendMailAsync(mailMessage);
                }
                catch (Exception ex)
                {
                    var errorMessage = new StringBuilder();
                    errorMessage.AppendLine("\r\n");
                    errorMessage.AppendLine($"Ошибка: {ex.Message}");
                    errorMessage.AppendLine($"Трассировка стэка: {ex.StackTrace}");
                    Logger.Error(errorMessage.ToString);
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