using System.Collections.Generic;
using HomeAccountingSystem_WebUI.Abstract;
using HomeAccountingSystem_WebUI.Infrastructure;

namespace HomeAccountingSystem_WebUI.Concrete
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
            return hasValue? outMessage : outMessage = "";
        }

        void FillDictionary()
        {
            _messages.Add(MessagesEnum.UserHasNoAccounts, "Сначала необходимо завести хотя бы один счет.");
            _messages.Add(MessagesEnum.UserHasNoCategories, "Сначала необходимо завести хотя бы одну категорию.");
        }
    }
}