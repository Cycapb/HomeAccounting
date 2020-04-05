using System.Collections.Generic;
using DomainModels.Model;

namespace WebUI.Core.Models.PayingItemModels
{
    public class PayingItemsCollectionModel
    {
        public IEnumerable<PayingItem> PayingItems { get; set; }

        public PagingInfo PagingInfo { get; set; }
    }
}