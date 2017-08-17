using System;

namespace Services.Exceptions
{
    public class ServiceException:Exception
    {
        public ServiceException():base("Возникла ошибка в сервисе")
        {
            
        }

        public ServiceException(string message, Exception innerException):base(message, innerException)
        {
            
        }
    }
}
