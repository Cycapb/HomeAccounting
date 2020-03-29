using DomainModels.Model;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Abstract;
using WebUI.Models.PayingItemModels;

namespace WebUI.Helpers
{
    public class PayingItemUpdater : IPayingItemUpdater
    {
        private readonly IPayingItemService _payingItemService;
        private bool _disposed;

        public PayingItemUpdater(IPayingItemService payingItemService)
        {
            _payingItemService = payingItemService;            
        }

        public async Task<PayingItem> UpdatePayingItemFromViewModel(PayingItemEditModel model)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var sum = GetSumOfTheProducts(model);
            var payingItem = await _payingItemService.GetItemAsync(model.PayingItem.ItemID).ConfigureAwait(false);
            payingItem.Summ = sum;
            var comment = CreateCommentForPayingItem(model);

            payingItem.Comment = comment;
            payingItem.CategoryID = model.PayingItem.CategoryID;
            payingItem.AccountID = model.PayingItem.AccountID;

            payingItem.PayingItemProducts.Clear();

            if (model.ProductsInItem == null && model.ProductsNotInItem == null)
            {
                await _payingItemService.UpdateAsync(payingItem);
                return payingItem;
            }

            return await CreatePayingItemProduct(model, payingItem).ConfigureAwait(false);            
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _payingItemService.Dispose();
                }

                _disposed = true;
            }
        }

        private async Task<PayingItem> CreatePayingItemProduct(PayingItemEditModel model, PayingItem payingItem)
        {
            var payingItemProducts = new List<PayingItemProduct>();

            if (model.ProductsInItem != null)
            {
                payingItemProducts.AddRange(model.ProductsInItem.Where(x => x.ProductID != 0).Select(p => new PayingItemProduct()
                {
                    PayingItemId = payingItem.ItemID,
                    ProductId = p.ProductID,
                    Price = p.Price
                })
                .ToList());
            }

            if (model.ProductsNotInItem != null)
            {
                payingItemProducts.AddRange(model.ProductsNotInItem.Where(x => x.ProductID != 0).Select(p => new PayingItemProduct()
                {
                    PayingItemId = payingItem.ItemID,
                    ProductId = p.ProductID,
                    Price = p.Price
                }).ToList());
            }            

            foreach (var payingItemProduct in payingItemProducts)
            {
                payingItem.PayingItemProducts.Add(payingItemProduct);
            }

            await _payingItemService.UpdateAsync(payingItem);

            return payingItem;
        }        

        private decimal GetSumOfTheProducts(PayingItemEditModel model)
        {
            var products = new List<Product>();

            if (model.ProductsInItem == null && model.ProductsNotInItem == null)
            {
                return model.PayingItem.Summ;
            }

            if (model.ProductsInItem != null && model.ProductsNotInItem == null)
            {
                if (model.ProductsInItem.Any(x => x.ProductID != 0))
                {
                    products.AddRange(model.ProductsInItem.Where(x => x.ProductID != 0).ToList());

                    return products.Sum(x => x.Price);
                }

                return model.PayingItem.Summ;
            }

            if (model.ProductsInItem != null && model.ProductsNotInItem != null)
            {
                products.AddRange(model.ProductsInItem.Where(x => x.ProductID != 0).ToList());
                products.AddRange(model.ProductsNotInItem.Where(x => x.ProductID != 0).ToList());
            }

            return products.Sum(x => x.Price);
        }

        private string CreateCommentForPayingItem(PayingItemEditModel model)
        {
            var productsNames = new List<string>();
            var comment = string.Empty;

            if (model.ProductsInItem == null && model.ProductsNotInItem == null)
            {
                return model.PayingItem.Comment;
            }

            if (model.ProductsInItem != null && model.ProductsNotInItem == null)
            {
                if (model.ProductsInItem.Any(x => x.ProductID != 0))
                {
                    productsNames.AddRange(model.ProductsInItem.Where(x => x.ProductID != 0).Select(p => p.ProductName));

                    foreach (var productName in productsNames)
                    {
                        comment += productName + ", ";
                    }

                    return comment.Remove(comment.LastIndexOf(",", StringComparison.Ordinal));
                }

                return model.PayingItem.Comment;
            }

            if (model.ProductsInItem != null && model.ProductsNotInItem != null)
            {
                productsNames.AddRange(model.ProductsInItem.Where(x => x.ProductID != 0).Select(x => x.ProductName).ToList());
                productsNames.AddRange(model.ProductsNotInItem.Where(x => x.ProductID != 0).Select(x => x.ProductName).ToList());

                foreach (var productName in productsNames)
                {
                    comment += productName + ", ";
                }
            }

            return string.IsNullOrEmpty(comment) ? comment : comment.Remove(comment.LastIndexOf(",", StringComparison.Ordinal));
        }
    }
}