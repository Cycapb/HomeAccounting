using System;
using Loggers.Models;

namespace Loggers
{
    public interface IExceptionLogger
    {
        void LogException(Exception exception, MvcLoggingModel mvcLoggingModel);
    }
}