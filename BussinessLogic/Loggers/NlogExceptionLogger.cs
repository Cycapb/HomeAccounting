using System;
using System.Text;
using Loggers;
using Loggers.Models;
using NLog;

namespace BussinessLogic.Loggers
{
    public class NlogExceptionLogger:IExceptionLogger
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public void LogException(Exception exception, MvcLoggingModel mvcLoggingModel)
        {
            if (exception == null)
            {
                return;
            }
            var errorMessage = new StringBuilder();
            errorMessage.AppendLine("\r\n");

            if (mvcLoggingModel != null)
            {
                errorMessage.AppendLine($"Пользователь: {mvcLoggingModel.UserName}");
                errorMessage.AppendLine($"IP-адрес(а): {mvcLoggingModel.UserHostAddress}");
                if (mvcLoggingModel.RouteData != null)
                {
                    if (mvcLoggingModel.RouteData.ContainsKey("controller") && mvcLoggingModel.RouteData.ContainsKey("action"))
                    {
                        errorMessage.AppendLine($"Контроллер: {mvcLoggingModel.RouteData?["controller"]} Метод: {mvcLoggingModel.RouteData?["action"]}");
                    }
                }
            }
            
            FillInnerExceptions(errorMessage, exception);
            Logger.Error(errorMessage.ToString);
        }

        private void FillInnerExceptions(StringBuilder errorMessage, Exception exception)
        {
            errorMessage.AppendLine($"Ошибка: {exception.Message}");
            errorMessage.AppendLine($"Трассировка стэка: {exception.StackTrace}");
            errorMessage.AppendLine("----------------Конец исключения----------------");
            if (exception.InnerException != null)
            {
                FillInnerExceptions(errorMessage, exception.InnerException);
            }
        }
    }
}