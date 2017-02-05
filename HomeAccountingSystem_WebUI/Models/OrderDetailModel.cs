using System.Collections.Generic;
using DomainModels.Model;

namespace HomeAccountingSystem_WebUI.Models
{
    public class AddOrderDetailView
    {
        public int OrderId { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Product> Products { get; set; }

    }
}