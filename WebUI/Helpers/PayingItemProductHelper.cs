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
        private readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly IPayingItemProductService _pItemProductService;
        private readonly IProductService _productService;

        public PayingItemProductHelper(IPayingItemProductService pItemProductService, IProductService productService)
        {
            _pItemProductService = pItemProductService;
            _productService = productService;
        }

        public async Task CreatePayingItemProduct(PayingItemEditModel pItem)
        {
            try
            {
                var paiyngItemProducts = _pItemProductService.GetList(x => x.PayingItemID == pItem.PayingItem.ItemID);

                foreach (var item in paiyngItemProducts)
                {
                    await _pItemProductService.DeleteAsync(item.ItemID);
                }
                await _pItemProductService.SaveAsync();

                foreach (var item in pItem.PricesAndIdsInItem)
                {
                    if (item.Id != 0)
                    {
                        var pItemProd = new PaiyngItemProduct()
                        {
                            PayingItemID = pItem.PayingItem.ItemID,
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

        public async Task UpdatePayingItemProduct(PayingItemEditModel pItem)
        {
            try
            {
                foreach (var item in pItem.PricesAndIdsInItem)
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

                if (pItem.PricesAndIdsNotInItem != null)
                {
                    foreach (var item in pItem.PricesAndIdsNotInItem)
                    {
                        if (item.Id != 0)
                        {
                            var payingItemProduct = new PaiyngItemProduct()
                            {
                                PayingItemID = pItem.PayingItem.ItemID,
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

        public async Task CreatePayingItemProduct(PayingItemModel pItem)
        {
            try
            {
                foreach (var item in pItem.Products)
                {
                    if (item.ProductID != 0)
                    {
                        var pItemProd = new PaiyngItemProduct()
                        {
                            PayingItemID = pItem.PayingItem.ItemID,
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
                var sw = new Stopwatch();
                _logger.Debug("Started getting items with new logic");
                sw.Start();
                payingItemProducts = _pItemProductService.GetList(x => x.PayingItemID == payingItemId)
                    .ToList();
                products =
                    _productService.GetList().Where(x => x.CategoryID == model.PayingItem.CategoryID)
                    .ToList();
                sw.Stop();
                _logger.Debug($"Query ended in {sw.ElapsedMilliseconds} ms");
                _logger.Debug("Ended getting items with new logic");
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
                    .ToList();
                var productsNotInItem = payingItemProducts.Join(products,
                    x => x.ProductID,
                    y => y.ProductID,
                    (x, y) => y)
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