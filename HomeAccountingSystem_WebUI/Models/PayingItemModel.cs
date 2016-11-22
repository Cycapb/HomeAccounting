using System.Collections.Generic;
using HomeAccountingSystem_DAL.Model;

namespace HomeAccountingSystem_WebUI.Models
{
    public class PayingItemModel
    {
        public PayingItem PayingItem { get; set; }
        public IList<Product> Products { get; set; } 
    }
}