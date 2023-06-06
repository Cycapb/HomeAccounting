using WebUI.Core.Infrastructure;

namespace WebUI.Core.Abstract
{
    public interface IMessageProvider
    {
        string Get(MessagesEnum message);
    }
}
