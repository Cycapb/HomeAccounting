using System.ComponentModel.DataAnnotations;

namespace WebUI.Core.Infrastructure.Attributes
{
    public class TransferModelAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            int? val = (int)value;

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