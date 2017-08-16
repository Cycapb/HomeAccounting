﻿using System;

namespace Services.Exceptions
{
    [Serializable]
    public class SendEmailException:Exception
    {
        public SendEmailException():base("Невозможно отправить список по электронной почте")
        {
            
        }

        public SendEmailException(string message, Exception innerException) : base(message, innerException) { }
    }
}