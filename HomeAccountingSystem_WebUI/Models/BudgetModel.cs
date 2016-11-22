using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HomeAccountingSystem_WebUI.Models
{
    public class BudgetModel
    {
        public string TypeOfMoney { get; set; }
        public decimal Month { get; set; }
        public decimal Week { get; set; }
        public decimal Day { get; set; }
    }
}