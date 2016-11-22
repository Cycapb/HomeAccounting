using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAccountingSystem_DAL.Infrastructure
{
    public class CategoryModelAttribute:ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            int? val = null;
            val = (int) value;
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
