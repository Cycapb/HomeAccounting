using DomainModels.Model;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Abstract;
using WebUI.Models.PayingItemViewModels;

namespace WebUI.Helpers
{
    public class PayingItemUpdater : IPayingItemUpdater
    {
        private readonly IPayingItemService _payingItemService;

        public PayingItemUpdater(IPayingItemService payingItemService)
        {
            _payingItemService = payingItemService;
        }

        public async Task<PayingItem> UpdatePayingItemFromViewModel(PayingItemEditViewModel model)
        {
            var sum = GetSumFromProducts(model);
            var payingItem = await _payingItemService.GetItemAsync(model.PayingItem.ItemID);
            payingItem.Summ = sum == 0 ? payingItem.Summ : sum;
            var comment = CreateCommentForPayingItem(model);
            payingItem.Comment = string.IsNullOrEmpty(comment) ? payingItem.Comment : comment;

            return payingItem;
        }

        private decimal GetSumFromProducts(PayingItemEditViewModel model)
        {
            var products = new List<Product>();

            if (model.ProductsInItem != null)
            {
                products.AddRange(model.ProductsInItem.Where(x => x.ProductID != 0).ToList());
            }

            if (model.ProductsNotInItem != null)
            {
                products.AddRange(model.ProductsNotInItem.Where(x => x.ProductID != 0).ToList());
            }

            return products.Sum(x => x.Price);
        }

        private string CreateCommentForPayingItem(PayingItemEditViewModel model)
        {
            var productsNames = new List<string>();
            var comment = string.Empty;

            if (model.ProductsInItem != null)
            {
                productsNames.AddRange(model.ProductsInItem.Where(x => x.ProductID != 0).Select(p => p.ProductName).ToList());
            }

            if (model.ProductsNotInItem != null)
            {
                productsNames.AddRange(model.ProductsNotInItem.Where(x => x.ProductID != 0).Select(p => p.ProductName).ToList());
            }

            foreach (var productName in productsNames)
            {
                comment += productName + ", ";
            }

            return string.IsNullOrEmpty(comment) ? comment : comment.Remove(comment.LastIndexOf(",", StringComparison.Ordinal));
        }
    }
}
