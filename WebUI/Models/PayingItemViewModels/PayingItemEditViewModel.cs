using DomainModels.Model;
using System.Collections.Generic;

namespace WebUI.Models.PayingItemViewModels
{
    public class PayingItemEditViewModel
    {
        public PayingItem PayingItem { get; set; }
        public List<Product> ProductsInItem { get; set; }
        public List<Product> ProductsNotInItem { get; set; }
    }
}