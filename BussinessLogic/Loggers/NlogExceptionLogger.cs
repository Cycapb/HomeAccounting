using System;
using System.Text;
using Loggers;
using Loggers.Models;
using NLog;

namespace BussinessLogic.Loggers
{
    public class NlogExceptionLogger:IExceptionLogger
    {
        private readonly IExceptionLogger _exceptionLogger;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public NlogExceptionLogger(IExceptionLogger exceptionLogger)
        {
            _exceptionLogger = exceptionLogger;
        }

        public void LogException(Exception exception, MvcLoggingModel mvcLoggingModel)
        {
            var errorMessage = new StringBuilder();
            errorMessage.AppendLine("\r\n");
            errorMessage.AppendLine($"Пользователь: {mvcLoggingModel.UserName}");
            errorMessage.AppendLine($"IP-адрес: {mvcLoggingModel.UserHostAddress}");
            errorMessage.AppendLine($"Контроллер: {mvcLoggingModel.RouteData["controller"]} Метод: {mvcLoggingModel.RouteData["action"]}");
            errorMessage.AppendLine($"Ошибка: {exception.Message}");
            errorMessage.AppendLine($"Трассировка стэка: {exception.StackTrace}");
            errorMessage.AppendLine("----------------Конец исключения----------------");
            errorMessage.AppendLine("");
            errorMessage.AppendLine($"InnerException: {exception.InnerException?.Message}");
            errorMessage.AppendLine($"InnerException StackTrace: {exception.InnerException?.StackTrace}");
            errorMessage.AppendLine($"----------------Конец исключения----------------");
            errorMessage.AppendLine("");
            errorMessage.AppendLine($"InnerException: {exception.InnerException?.InnerException?.Message}");
            errorMessage.AppendLine($"InnerException StackTrace: {exception.InnerException?.InnerException?.StackTrace}");
            Logger.Error(errorMessage.ToString);
        }
    }
}
//ToDo Реализовать рекурсивный проход по всем эксепшенам внутри Exception