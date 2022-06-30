using DomainModels.Model;
using System;

namespace Providers
{
    /// <summary>
    /// Base supplier of email settings
    /// </summary>
    public interface IMailSettingsProvider : IDisposable
    {
        EmailSettings GetEmailSettings();
    }
}