using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DomainModels.Model;

namespace WebUI.Models.PayingItemViewModels
{
    public class PayingItemEditViewModel
    {        
        public PayingItem PayingItem { get; set; }
        public List<Product> ProductsInItem { get; set; }
        public List<Product> ProductsNotInItem { get; set; }
        public List<PriceAndIdForEdit> PricesAndIdsInItem { get; set; }
        public List<PriceAndIdForEdit> PricesAndIdsNotInItem { get; set; }
    }

    public class PriceAndIdForEdit
    {        
        public int Id { get; set; }
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
    }
}