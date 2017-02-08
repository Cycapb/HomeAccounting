using DomainModels.Model;

namespace Services
{
    /// <summary>
    /// Базовый поставщик настроек электронной почты
    /// </summary>
    public interface IMailSettingsProvider
    {
        EmailSettings GetEmailSettings();
    }
}