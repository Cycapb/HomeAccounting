using System;

namespace BussinessLogic.Exceptions
{
    public class SendEmailException:Exception
    {
        public SendEmailException():base("Невозможно отправить список по электронной почте")
        {
            
        }
    }
}