using System.Collections.Generic;
using DomainModels.Model;

namespace WebUI.Core.Models.PayingItemModels
{
    public class PayingItemModel
    {
        public PayingItem PayingItem { get; set; }

        public IList<Product> Products { get; set; } 
    }
}