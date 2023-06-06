using System.Collections.Generic;
using DomainModels.Model;

namespace WebUI.Core.Models.PayingItemModels
{
    public class PayingItemsListWithPaginationModel
    {
        public IEnumerable<PayingItem> PayingItems { get; set; }

        public PagingInfo PagingInfo { get; set; }
    }
}