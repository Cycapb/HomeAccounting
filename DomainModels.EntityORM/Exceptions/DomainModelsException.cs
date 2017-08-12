using System;

namespace DomainModels.EntityORM.Exceptions
{
    public class DomainModelsException:Exception
    {
        public DomainModelsException() : base("Возникла ошибка на уровне доступа к данным")
        {
            
        }

        public DomainModelsException(string message, Exception innerException):base(message, innerException)
        {
            
        }
    }
}
