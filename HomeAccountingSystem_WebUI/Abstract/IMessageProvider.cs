using HomeAccountingSystem_WebUI.Infrastructure;

namespace HomeAccountingSystem_WebUI.Abstract
{
    public interface IMessageProvider
    {
        string Get(MessagesEnum message);
    }
}
