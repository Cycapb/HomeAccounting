namespace Services
{
    /// <summary>
    /// Служит для отправки сообщения по электронной почте
    /// </summary>
    public interface IEmailSender
    {
        void Send(string message, string mailTo);
        string MailTo { get; }
    }
}