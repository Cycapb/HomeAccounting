using System.ComponentModel.DataAnnotations;

namespace HomeAccountingSystem_WebUI.Infrastructure
{
    public class TransferModelAttribute:ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            int? val = null;
            val = (int)value;

            if (val.Value == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}