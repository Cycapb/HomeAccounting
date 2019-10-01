﻿using DomainModels.Model;
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
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var sum = GetSumOfTheProducts(model);
            var payingItem = await _payingItemService.GetItemAsync(model.PayingItem.ItemID).ConfigureAwait(false);
            payingItem.Summ = sum == 0 ? model.PayingItem.Summ : sum;
            var comment = CreateCommentForPayingItem(model);
            payingItem.Comment = string.IsNullOrEmpty(comment) ? model.PayingItem.Comment : comment;
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

            foreach (var payingItemProduct in payingItemProducts)
            {
                payingItem.PayingItemProducts.Add(payingItemProduct);
            }

            await _payingItemService.UpdateAsync(payingItem);

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