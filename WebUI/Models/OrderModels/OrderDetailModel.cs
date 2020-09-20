using System.Collections.Generic;
using DomainModels.Model;

namespace WebUI.Models.OrderModels
{
    public class OrderDetailModel
    {
        public int OrderId { get; set; }

        public IEnumerable<Category> Categories { get; set; }

        public IEnumerable<Product> Products { get; set; }
    }
}