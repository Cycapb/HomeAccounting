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
            var sum = GetSumOfTheProducts(model);
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

            return await CreatePayingItemProduct(model, payingItem);            
        }

        private async Task<PayingItem> CreatePayingItemProduct(PayingItemEditViewModel model, PayingItem payingItem)
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

            payingItem.PayingItemProducts.Clear();

            foreach (var payingItemProduct in payingItemProducts)
            {
                payingItem.PayingItemProducts.Add(payingItemProduct);
            }

            await _payingItemService.SaveAsync();

            return payingItem;
        }        

        private decimal GetSumOfTheProducts(PayingItemEditViewModel model)
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

//ToDo Write unit tests for the PayingItemUpdaterClass
