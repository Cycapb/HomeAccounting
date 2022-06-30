using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using DomainModels.Model;
using Providers;
using Services;
using Services.Exceptions;

namespace BusinessLogic.Services
{
    public class EmailSenderService : IEmailSender
    {
        private readonly IMailSettingsProvider _mailSettingsProvider;
        private bool _disposed;

        public EmailSenderService(IMailSettingsProvider mailSettingsProvider)
        {
            _mailSettingsProvider = mailSettingsProvider;
        }

        public async Task SendAsync(string message, string subject, string mailTo)
        {
            var mailSettings = _mailSettingsProvider.GetEmailSettings();

            if (mailSettings != null)
            {
                mailSettings.MailTo = mailTo;
                var smtpClient = CreateSmtpClient(mailSettings);

                try
                {
                    var mailMessage = new MailMessage(mailSettings.MailFrom, mailSettings.MailTo, subject, message);
                    await smtpClient.SendMailAsync(mailMessage);
                }
                catch (Exception ex)
                {
                    throw new SendEmailException($"Возникла ошибка в сервисе {nameof(EmailSenderService)} в методе {nameof(SendAsync)} при отправке почты", ex);
                }
                finally
                {
                    smtpClient.Dispose();
                }                  
            }
            else
            {
                throw new SendEmailException($"Возникла ошибка в сервисе {nameof(EmailSenderService)} в методе {nameof(SendAsync)} при отправке почты. " +
                                             "Ошибка: Невозможно получить учетные данные почтового ящика для отправки списка покупок!", null);
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
                    _mailSettingsProvider.Dispose();
                }

                _disposed = true;
            }
        }
    }
}