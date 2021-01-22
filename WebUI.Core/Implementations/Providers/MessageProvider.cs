using System.Collections.Generic;
using WebUI.Core.Abstract;
using WebUI.Core.Infrastructure;

namespace WebUI.Core.Implementations.Providers
{
    public class MessageProvider : IMessageProvider
    {
        private readonly Dictionary<MessagesEnum, string> _messages;

        public MessageProvider()
        {
            _messages = new Dictionary<MessagesEnum, string>();
            FillDictionary();
        }

        public string Get(MessagesEnum message)
        {
            string outMessage;
            var hasValue = _messages.TryGetValue(message, out outMessage);
            return hasValue ? outMessage : "";
        }

        private void FillDictionary()
        {
            _messages.Add(MessagesEnum.UserHasNoAccounts, "Сначала необходимо завести хотя бы один счет.");
            _messages.Add(MessagesEnum.UserHasNoCategories, "Сначала необходимо завести хотя бы одну категорию.");
        }
    }
}