using WebUI.Core.Infrastructure.Attributes;

namespace WebUI.Core.Models.MailboxModels
{
    public class AddNotificationMailboxModel : NotificationMailboxModelBase
    {
        [IsUnique(ErrorMessage = "Такой почтовый ящик уже существует")]
        public override string MailBoxName { get; set; }
    }
}