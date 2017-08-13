using System;

namespace BussinessLogic.Exceptions
{
    public class ServiceException:Exception
    {
        public ServiceException(string message, Exception innerException):base(message, innerException)
        {
            
        }
    }
}
