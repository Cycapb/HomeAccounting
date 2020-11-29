using System.Collections.Generic;
using WebUI.Abstract;
using WebUI.Infrastructure;

namespace WebUI.Concrete
{
    public class MessageProvider : IMessageProvider
    {
        Dictionary<MessagesEnum, string> _messages;

        public MessageProvider()
        {
            _messages = new Dictionary<MessagesEnum, string>();
            FillDictionary();
        }

        public string Get(MessagesEnum message)
        {
            string outMessage;
            var hasValue = _messages.TryGetValue(message, out outMessage);
            return hasValue? outMessage : "";
        }

        void FillDictionary()
        {
            _messages.Add(MessagesEnum.UserHasNoAccounts, "Сначала необходимо завести хотя бы один счет.");
            _messages.Add(MessagesEnum.UserHasNoCategories, "Сначала необходимо завести хотя бы одну категорию.");
        }
    }
}