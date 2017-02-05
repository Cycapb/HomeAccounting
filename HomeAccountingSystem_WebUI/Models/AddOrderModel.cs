using System;
using System.Collections.Generic;
using DomainModels.Model;

namespace HomeAccountingSystem_WebUI.Models
{
    public class AddOrderModel
    {
        public DateTime OrderDate { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}