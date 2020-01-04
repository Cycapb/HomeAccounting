using DomainModels.Model;

namespace Services
{
    /// <summary>
    /// Base supplier of email settings
    /// </summary>
    public interface IMailSettingsProvider
    {
        EmailSettings GetEmailSettings();
    }
}