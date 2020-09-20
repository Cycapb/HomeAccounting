using System;

namespace WebUI.Core.Exceptions
{
    public class WebUiException:Exception
    {
        public WebUiException():base("Ошибка в контроллере")
        {
            
        }

        public WebUiException(string message, Exception innerException):base(message, innerException)
        {
                
        }
    }
}