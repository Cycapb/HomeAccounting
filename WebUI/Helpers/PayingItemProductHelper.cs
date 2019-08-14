using DomainModels.Model;
using Services;
using Services.Exceptions;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Abstract;
using WebUI.Exceptions;
using WebUI.Models;

namespace WebUI.Helpers
{
    public class PayingItemProductHelper : IPayingItemProductHelper
    {
        private readonly IPayingItemProductService _pItemProductService;
        private readonly IProductService _productService;

        public PayingItemProductHelper(IPayingItemProductService pItemProductService, IProductService productService)
        {
            _pItemProductService = pItemProductService;
            _productService = productService;
        }

        public async Task CreatePayingItemProduct(PayingItemEditModel model)
        {
            try
            {
                var payingItemProducts = _pItemProductService.GetList(x => x.PayingItemID == model.PayingItem.ItemID);

                foreach (var item in payingItemProducts)
                {
                    await _pItemProductService.DeleteAsync(item.ItemID);
                }
                await _pItemProductService.SaveAsync();

                foreach (var item in model.PricesAndIdsInItem)
                {
                    if (item.Id != 0)
                    {
                        var pItemProd = new PaiyngItemProduct()
                        {
                            PayingItemID = model.PayingItem.ItemID,
                            Summ = item.Price,
                            ProductID = item.Id
                        };
                        await _pItemProductService.CreateAsync(pItemProd);
                    }
                }
                await _pItemProductService.SaveAsync();
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
                foreach (var item in model.PricesAndIdsInItem)
                {
                    if (item.Id != 0)
                    {
                        var itemToUpdate = await _pItemProductService.GetItemAsync(item.PayingItemProductId);
                        if (itemToUpdate != null)
                        {
                            itemToUpdate.Summ = item.Price;
                            await _pItemProductService.UpdateAsync(itemToUpdate);
                        }
                    }
                    if (item.Id == 0 && item.Price != 0)
                    {
                        await _pItemProductService.DeleteAsync(item.PayingItemProductId);
                    }
                }
                await _pItemProductService.SaveAsync();

                if (model.PricesAndIdsNotInItem != null)
                {
                    foreach (var item in model.PricesAndIdsNotInItem)
                    {
                        if (item.Id != 0)
                        {
                            var payingItemProduct = new PaiyngItemProduct()
                            {
                                PayingItemID = model.PayingItem.ItemID,
                                Summ = item.Price,
                                ProductID = item.Id
                            };
                            await _pItemProductService.CreateAsync(payingItemProduct);
                        }
                    }
                    await _pItemProductService.SaveAsync();
                }
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
                        await _pItemProductService.CreateAsync(pItemProd);
                        await _pItemProductService.SaveAsync();
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
            var payingItemProducts = new List<PaiyngItemProduct>();
            var products = new List<Product>();

            try
            {
                payingItemProducts = _pItemProductService.GetList(x => x.PayingItemID == payingItemId)
                    .ToList();
                products =
                    _productService.GetList(x => x.CategoryID == model.PayingItem.CategoryID)
                    .ToList();
            }
            catch (ServiceException e)
            {
                throw new WebUiHelperException(
                    $"Ошибка в типе {nameof(PayingItemProductHelper)} в методе {nameof(FillPayingItemEditModel)}", e);
            }

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
    }
}