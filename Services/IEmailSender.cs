namespace Services
{
    public interface IEmailSender
    {
        void Send(string message);
    }
}