using DomainModels.Model;
using System;

namespace Services
{
    /// <summary>
    /// Base supplier of email settings
    /// </summary>
    public interface IMailSettingsProvider : IDisposable
    {
        EmailSettings GetEmailSettings();
    }
}