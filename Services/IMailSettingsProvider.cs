using DomainModels.Model;

namespace Services
{
    public interface IMailSettingsProvider
    {
        EmailSettings GetEmailSettings();
    }
}