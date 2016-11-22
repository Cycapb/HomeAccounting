using System.Configuration;
using HomeAccountingSystem_DAL.Model;
using Services;

namespace BussinessLogic.Providers
{
    public class EmailSettingsProvider:IMailSettingsProvider
    {
        public EmailSettings GetEmailSettings()
        {
            return new EmailSettings()
            {
                MailFrom = ConfigurationManager.AppSettings["MailFrom"],
                UserName = ConfigurationManager.AppSettings["UserName"],
                Password = ConfigurationManager.AppSettings["Password"],
                ServerName = ConfigurationManager.AppSettings["Server"],
                ServerPort = int.Parse(ConfigurationManager.AppSettings["Port"]),
                UseSsl = bool.Parse(ConfigurationManager.AppSettings["UseSsl"])
            };
        }
    }
}