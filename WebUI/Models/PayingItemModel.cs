using System.Collections.Generic;
using DomainModels.Model;

namespace WebUI.Models
{
    public class PayingItemViewModel
    {
        public PayingItem PayingItem { get; set; }
        public IList<Product> Products { get; set; } 
    }
}