using System;
using System.Threading.Tasks;

namespace Services
{
    /// <summary>
    /// Is used for sending emails
    /// </summary>
    public interface IEmailSender : IDisposable
    {
        Task SendAsync(string message, string subject, string mailTo);
    }
}