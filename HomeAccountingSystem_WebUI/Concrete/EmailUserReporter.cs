using System;
using System.Text;
using System.Threading.Tasks;
using HomeAccountingSystem_WebUI.Abstract;
using HomeAccountingSystem_WebUI.Models;
using NLog;

namespace HomeAccountingSystem_WebUI.Concrete
{
    public class EmailUserReporter:IReporter
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public async Task Report(AccUserModel user, string address)
        {
                StringBuilder messageBody = new StringBuilder();
                if (user.UserName.ToLower() == "Demo".ToLower())
                {
                    DemoUser(messageBody,address);
                    await Task.Run(() => { Logger.Warn(messageBody.ToString); });
                }
                else
                {
                    messageBody.AppendLine("Зарегистрировался новый пользователь");
                    messageBody.AppendLine("Логин: " + user.UserName);
                    messageBody.AppendLine("Email: " + user.Email);
                    messageBody.AppendLine("IP: " + address);
                    messageBody.AppendLine("Дата регистрации: " + DateTime.Now);
                    await Task.Run(() => { Logger.Warn(messageBody.ToString); });
            }
        }

        private void DemoUser(StringBuilder messageBody, string address)
        {
            messageBody.AppendLine("В систему зашел пользователь Demo");
            messageBody.AppendLine("IP входа: " + address);
            messageBody.AppendLine("Дата входа: " + DateTime.Now);
        }
    }
}