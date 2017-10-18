using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DomainModels.Model;

namespace WebUI.Models
{
    public class PayingItemEditModel
    {
        public static int OldCategoryId { get; set; }
        public PayingItem PayingItem { get; set; }
        public List<IdNamePrice> ProductsInItem { get; set; }
        public List<Product> ProductsNotInItem { get; set; }
        public List<PriceAndIdForEdit> PricesAndIdsInItem { get; set; }
        public List<PriceAndIdForEdit> PricesAndIdsNotInItem { get; set; } 
        public List<PaiyngItemProduct> PayingItemProducts { get; set; }
    }

    public class IdNamePrice
    {
        public int PayingItemProductId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
    }

    public class PriceAndIdForEdit
    {
        public int PayingItemProductId { get; set; }
        public int Id { get; set; }
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
    }

}