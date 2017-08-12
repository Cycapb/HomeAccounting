using System;

namespace WebUI.Exceptions
{
    public class WebUIException:Exception
    {
        public WebUIException(string message, Exception innerException):base(message, innerException)
        {
                
        }
    }
}