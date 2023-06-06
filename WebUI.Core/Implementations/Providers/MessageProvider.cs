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
            var hasValue = _messages.TryGetValue(message, out var outMessage);

            return hasValue ? outMessage : "";
        }

        private void FillDictionary()
        {
            _messages.Add(MessagesEnum.UserHasNoAccounts, "Сначала необходимо завести хотя бы один счет");
            _messages.Add(MessagesEnum.UserHasNoCategories, "Сначала необходимо завести хотя бы одну категорию");
            _messages.Add(MessagesEnum.UserHasNoExpenseCategoriesWithPurchases, "Сначала необходимо совершить покупку, используя подкатегории");
        }
    }
}