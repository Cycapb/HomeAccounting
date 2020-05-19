using NLog;
using System;
using System.Text;
using System.Threading.Tasks;
using WebUI.Abstract;
using WebUI.Models;

namespace WebUI.Concrete
{
    public class UserLoginActivityLogger : IUserLoginActivityLogger
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public async Task Log(AccUserModel user, string ipAddress)
        {
            var messageBody = CreateMessageBody(user, ipAddress);

            await Task.Run(() => Logger.Info(messageBody.ToString));
        }

        private StringBuilder CreateMessageBody(AccUserModel user, string ipAddress)
        {
            if (!(user.UserName.ToLower() == "Demo".ToLower()))
            {                
                return ExistingUser(ipAddress, user);
            }

            return DemoUser(ipAddress);
        }

        private StringBuilder DemoUser(string address)
        {
            var messageBody = new StringBuilder();
            messageBody.AppendLine("В систему вошел пользователь Demo");
            messageBody.AppendLine("IP входа: " + address);
            messageBody.AppendLine("Дата входа: " + DateTime.Now);

            return messageBody;
        }

        private StringBuilder ExistingUser(string address, AccUserModel user)
        {
            var messageBody = new StringBuilder();
            messageBody.AppendLine("Зарегистрировался новый пользователь");
            messageBody.AppendLine("Логин: " + user.UserName);
            messageBody.AppendLine("Email: " + user.Email);
            messageBody.AppendLine("IP: " + address);
            messageBody.AppendLine("Дата регистрации: " + DateTime.Now);

            return messageBody;
        }
    }
}