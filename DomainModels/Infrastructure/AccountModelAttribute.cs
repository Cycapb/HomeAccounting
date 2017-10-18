using System.ComponentModel.DataAnnotations;

namespace DomainModels.Infrastructure
{
    public class AccountModelAttribute : ValidationAttribute
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
