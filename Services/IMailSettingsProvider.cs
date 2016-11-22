using HomeAccountingSystem_DAL.Model;

namespace Services
{
    public interface IMailSettingsProvider
    {
        EmailSettings GetEmailSettings();
    }
}