using System.Threading.Tasks;

namespace Services
{
    /// <summary>
    /// Is used for sending emails
    /// </summary>
    public interface IEmailSender
    {
        Task SendAsync(string message, string mailTo);
        string MailTo { get; }
    }
}