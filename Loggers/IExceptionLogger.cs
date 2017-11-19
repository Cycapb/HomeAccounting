using System;
using Loggers.Models;

namespace Loggers
{
    public interface IExceptionLogger
    {
        /// <summary>
        /// Логгирует exception с любой глубиной вложенности InnerException.
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="mvcLoggingModel">Может быть null</param>
        void LogException(Exception exception, MvcLoggingModel mvcLoggingModel = null);
    }
}