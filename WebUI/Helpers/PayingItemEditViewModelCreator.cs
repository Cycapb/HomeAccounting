﻿using DomainModels.Model;
using Services;
using Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Abstract;
using WebUI.Exceptions;
using WebUI.Infrastructure.Comparers;
using WebUI.Models;
using WebUI.Models.PayingItemModels;

namespace WebUI.Helpers
{
    public class PayingItemEditViewModelCreator : IPayingItemEditViewModelCreator
    {
        private readonly IPayingItemService _payingItemService;
        private bool _disposed;

        public PayingItemEditViewModelCreator(IPayingItemService payingItemService)
        {
            _payingItemService = payingItemService;
        }

        public async Task<PayingItemEditModel> CreateViewModel(int payingItemId)
        {
            try
            {
                var payingItem = await _payingItemService.GetItemAsync(payingItemId);

                if (payingItem == null)
                {
                    return null;
                }

                var model = new PayingItemEditModel()
                {
                    PayingItem = payingItem,
                    ProductsInItem = new List<Product>(),
                    ProductsNotInItem = new List<Product>()
                };
                
                var productsInItem = payingItem.PayingItemProducts
                    .Select(x => new Product()
                    {
                        Price = x.Price,
                        ProductID = x.Product.ProductID,
                        Description = x.Product.Description,
                        ProductName = x.Product.ProductName,
                        UserID = x.Product.UserID
                    })
                    .ToList();
                var productsByCategory = payingItem.Category.Products.ToList();                

                if (productsInItem.Count != 0)
                {
                    model.ProductsInItem = productsInItem;
                    model.ProductsNotInItem = productsByCategory.Except(productsInItem, new ProductEqualityComparer()).ToList();
                }
                else
                {
                    model.ProductsNotInItem = productsByCategory;
                }

                return model;
            }
            catch (ServiceException e)
            {
                throw new WebUiException(
                    $"Ошибка в типе {nameof(PayingItemEditViewModelCreator)} в методе {nameof(CreateViewModel)}", e);
            }

            catch (Exception e)
            {
                throw new WebUiException(
                    $"Ошибка в типе {nameof(PayingItemEditViewModelCreator)} в методе {nameof(CreateViewModel)}", e);
            }
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
    }
}