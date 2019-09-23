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
        private readonly IPayingItemProductService _payingItemProductService;

        public PayingItemUpdater(IPayingItemService payingItemService, IPayingItemProductService payingItemProductService)
        {
            _payingItemService = payingItemService;
            _payingItemProductService = payingItemProductService;
        }

        public async Task<PayingItem> UpdatePayingItemFromViewModel(PayingItemEditViewModel model)
        {
            var sum = GetSumFromProducts(model);
            var payingItem = await _payingItemService.GetItemAsync(model.PayingItem.ItemID);
            payingItem.Summ = sum == 0 ? payingItem.Summ : sum;
            var comment = CreateCommentForPayingItem(model);
            payingItem.Comment = string.IsNullOrEmpty(comment) ? payingItem.Comment : comment;
            payingItem.CategoryID = model.PayingItem.CategoryID;
            payingItem.AccountID = model.PayingItem.AccountID;           

            if (model.ProductsInItem == null && model.ProductsNotInItem == null)
            {
                await _payingItemService.SaveAsync();
                return payingItem;
            }
            await CreatePayingItemProduct(model, payingItem);

            return payingItem;
        }

        private async Task CreatePayingItemProduct(PayingItemEditViewModel model, PayingItem payingItem)
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

            if (payingItem.PayingItemProducts.Count != 0)
            {
                payingItem.PayingItemProducts.Clear();
            }

            foreach (var product in products)
            {
                payingItem.PayingItemProducts.Add(new PayingItemProduct()
                {
                    PayingItemId = payingItem.ItemID,
                    ProductId = product.ProductID,
                    Price = product.Price
                });
            }

            await _payingItemService.SaveAsync();
        }

        private async Task CreatePayingItemProducts(PayingItemEditViewModel model, PayingItem payingItem)
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

            foreach (var item in payingItem.PaiyngItemProducts)
            {
                await _payingItemProductService.DeleteAsync(item.ItemID);
            }

            foreach (var product  in products)
            {
                await _payingItemProductService.CreateAsync(new PaiyngItemProduct()
                {
                    PayingItemID = model.PayingItem.ItemID,
                    ProductID = product.ProductID,
                    Summ = product.Price
                });
            }

            await _payingItemProductService.SaveAsync();
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
