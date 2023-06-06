using System;

namespace WebUI.Core.Exceptions
{
    public class WebUiHelperException:Exception
    {
        public WebUiHelperException():base("Ошибка в хэлпере")
        {

        }

        public WebUiHelperException(string message, Exception innerException):base(message, innerException)
        {

        }
    }
}