using DomainModels.Model;
using Services;
using Services.Exceptions;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Abstract;
using WebUI.Exceptions;
using WebUI.Models;

namespace WebUI.Helpers
{
    public class PayingItemProductHelper : IPayingItemProductHelper
    {
        private readonly IPayingItemProductService _payingItemProductService;
        private readonly IProductService _productService;

        public PayingItemProductHelper(IPayingItemProductService payingItemProductService, IProductService productService)
        {
            _payingItemProductService = payingItemProductService;
            _productService = productService;
        }

        public async Task CreatePayingItemProduct(PayingItemEditModel model)
        {
            try
            {
                var payingItemProducts = _payingItemProductService.GetList(x => x.PayingItemID == model.PayingItem.ItemID);

                foreach (var item in payingItemProducts)
                {
                    await _payingItemProductService.DeleteAsync(item.ItemID);
                }
                await _payingItemProductService.SaveAsync();

                foreach (var item in model.PricesAndIdsInItem)
                {
                    if (item.Id != 0)
                    {                        
                        var payingItemProduct = CreateItem(model.PayingItem.ItemID, item.Id, item.Price);
                        await _payingItemProductService.CreateAsync(payingItemProduct);
                    }
                }
                await _payingItemProductService.SaveAsync();
            }
            catch (ServiceException e)
            {
                throw new WebUiHelperException(
                    $"Ошибка в типе {nameof(PayingItemProductHelper)} в методе {nameof(CreatePayingItemProduct)}", e);
            }
        }

        public async Task UpdatePayingItemProduct(PayingItemEditModel model)
        {
            try
            {
                var payingItemProducts = _payingItemProductService.GetList(x => x.PayingItemID == model.PayingItem.ItemID).ToList();

                foreach (var item in payingItemProducts)
                {
                    await _payingItemProductService.DeleteAsync(item.ItemID);
                }

                await _payingItemProductService.SaveAsync();

                foreach (var item in model.PricesAndIdsInItem)
                {
                    if (item.Id != 0)
                    {
                        var payingItemProduct = CreateItem(model.PayingItem.ItemID, item.Id, item.Price);
                        await _payingItemProductService.CreateAsync(payingItemProduct);
                    }                                  
                }                

                if (model.PricesAndIdsNotInItem != null)
                {
                    foreach (var item in model.PricesAndIdsNotInItem)
                    {
                        if (item.Id != 0)
                        {
                            var payingItemProduct = CreateItem(model.PayingItem.ItemID, item.Id, item.Price);
                            await _payingItemProductService.CreateAsync(payingItemProduct);
                        }
                    }                    
                }

                await _payingItemProductService.SaveAsync();
            }
            catch (ServiceException e)
            {
                throw new WebUiHelperException(
                    $"Ошибка в типе {nameof(PayingItemProductHelper)} в методе {nameof(UpdatePayingItemProduct)}", e);
            }
        }

        public async Task CreatePayingItemProduct(PayingItemModel model)
        {
            try
            {
                foreach (var item in model.Products)
                {
                    if (item.ProductID != 0)
                    {
                        var pItemProd = new PaiyngItemProduct()
                        {
                            PayingItemID = model.PayingItem.ItemID,
                            Summ = item.Price,
                            ProductID = item.ProductID
                        };
                        await _payingItemProductService.CreateAsync(pItemProd);
                        await _payingItemProductService.SaveAsync();
                    }
                }
            }
            catch (ServiceException e)
            {
                throw new WebUiHelperException(
                    $"Ошибка в типе {nameof(PayingItemProductHelper)} в методе {nameof(CreatePayingItemProduct)}", e);
            }
        }

        public void FillPayingItemEditModel(PayingItemEditModel model, int payingItemId)
        {
            try
            {
                var payingItemProducts = _payingItemProductService.GetList(x => x.PayingItemID == payingItemId)
                    .ToList();
                var products =
                    _productService.GetList(x => x.CategoryID == model.PayingItem.CategoryID)
                    .ToList();

                model.PayingItemProducts = payingItemProducts;

                if (payingItemProducts.Count != 0)
                {
                    var productsInItem = payingItemProducts.Join(products,
                            x => x.ProductID,
                            y => y.ProductID,
                            (x, y) => new IdNamePrice()
                            {
                                PayingItemProductId = x.ItemID,
                                ProductId = x.ProductID,
                                ProductName = y.ProductName,
                                ProductDescription = y.Description,
                                Price = x.Summ
                            })
                        .OrderBy(x => x.ProductName)
                        .ToList();

                    var productsNotInItem = payingItemProducts.Join(products,
                            x => x.ProductID,
                            y => y.ProductID,
                            (x, y) => y)
                        .OrderBy(x => x.ProductName)
                        .ToList();

                    model.ProductsInItem = productsInItem;
                    model.ProductsNotInItem = products.Except(productsNotInItem).ToList();
                }
                else
                {
                    model.ProductsNotInItem = products;
                }
            }
            catch (ServiceException e)
            {
                throw new WebUiHelperException(
                    $"Ошибка в типе {nameof(PayingItemProductHelper)} в методе {nameof(FillPayingItemEditModel)}", e);
            }

            catch (Exception e)
            {
                throw new WebUiHelperException(
                    $"Ошибка в типе {nameof(PayingItemProductHelper)} в методе {nameof(FillPayingItemEditModel)}", e);
            }
        }

        private PaiyngItemProduct CreateItem(int payingItemId, int productId, decimal price)
        {
            return new PaiyngItemProduct()
            {
                PayingItemID = payingItemId,
                ProductID = productId,
                Summ = price
            };
        }
    }
}