using DomainModels.Model;
using System.Collections.Generic;

namespace WebUI.Core.Models.PayingItemModels
{
    public class PayingItemEditModel
    {
        public PayingItem PayingItem { get; set; }

        public List<Product> ProductsInItem { get; set; }

        public List<Product> ProductsNotInItem { get; set; }
    }
}