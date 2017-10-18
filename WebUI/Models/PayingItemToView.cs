using System.Collections.Generic;
using DomainModels.Model;

namespace WebUI.Models
{
    public class PayingItemToView
    {
        public IEnumerable<PayingItem> PayingItems { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
    
}