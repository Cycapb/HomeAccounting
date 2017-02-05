using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using DomainModels.Model;
using Services;
using NLog;

namespace BussinessLogic.Services
{
    public class EmailSenderService:IEmailSender
    {
        private readonly IMailSettingsProvider _mailSettingsProvider;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static string MailTo { get; set; }

        public EmailSenderService(IMailSettingsProvider mailSettingsProvider)
        {
            _mailSettingsProvider = mailSettingsProvider;
        }

        public async void Send(string message)
        {
            var mailSettings = _mailSettingsProvider.GetEmailSettings();
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