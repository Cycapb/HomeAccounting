using WebUI.Infrastructure;

namespace WebUI.Abstract
{
    public interface IMessageProvider
    {
        string Get(MessagesEnum message);
    }
}
