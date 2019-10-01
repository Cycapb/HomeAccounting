using System.Threading.Tasks;

namespace Services
{
    /// <summary>
    /// Служит для отправки сообщения по электронной почте
    /// </summary>
    public interface IEmailSender
    {
        Task SendAsync(string message, string mailTo);
        string MailTo { get; }
    }
}