using DomainModels.EntityORM;
using DomainModels.Model;
using NLog;
using Services;
using Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Abstract;
using WebUI.Exceptions;
using WebUI.Models;

namespace WebUI.Helpers
{
    public class PayingItemHelper : IPayingItemHelper
    {
        private readonly IProductService _productService;
        private readonly IPayingItemProductService _payingItemProductService;
        private readonly IPayingItemService _payingItemService;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public PayingItemHelper(IProductService productService, IPayingItemProductService payingItemProductService, IPayingItemService payingItemService)
        {
            _productService = productService;
            _payingItemProductService = payingItemProductService;
            _payingItemService = payingItemService;
        }        

        public void CreateCommentWhileEdit(PayingItemEditModel model)
        {
            var products = new List<Product>();
            try
            {
                products = _productService.GetList(x => x.CategoryID == model.PayingItem.CategoryID).ToList();
                model.PayingItem.Comment = "";
                var comment = string.Empty;

                foreach (var item in model.PricesAndIdsInItem)
                {
                    if (item.Id != 0)
                    {
                        comment += products.Single(x => x.ProductID == item.Id).ProductName + ", ";
                    }
                }

                if (model.PricesAndIdsNotInItem != null)
                {
                    foreach (var item in model.PricesAndIdsNotInItem)
                    {
                        if (item.Id != 0)
                        {
                            comment += products.Single(x => x.ProductID == item.Id).ProductName + ", ";
                        }
                    }
                }

                if (!string.IsNullOrEmpty(comment))
                {
                    model.PayingItem.Comment = comment.Remove(comment.LastIndexOf(","));
                }
            }
            catch (ServiceException e)
            {
                throw new WebUiHelperException(
                    $"Ошибка в типе {nameof(PayingItemHelper)} в методе {nameof(CreateCommentWhileEdit)}", e);
            }
        }

        public async Task CreatePayingItem(PayingItemModel model)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            try
            {
                if (model.Products != null)
                {
                    var sum = model.Products.Sum(x => x.Price);
                    if (sum != 0)
                    {
                        model.PayingItem.Summ = sum;
                    }

                    CreateCommentWhileAdd(model);

                    var itemsToAdd = model.Products.Where(x => x.ProductID != 0)
                        .Select(product => new PaiyngItemProduct()
                        {
                            PayingItemID = model.PayingItem.ItemID,
                            ProductID = product.ProductID,
                            Summ = product.Price
                        });

                    foreach (var item in itemsToAdd)
                    {
                        model.PayingItem.PaiyngItemProducts.Add(item);
                    }
                }
                await _payingItemService.CreateAsync(model.PayingItem);
            }
            catch (ServiceException e)
            {
                throw new WebUiHelperException(
                    $"Ошибка в типе {nameof(PayingItemHelper)} в методе {nameof(CreatePayingItemProducts)}", e);
            }
        }

        public async Task CreatePayingItemProducts(PayingItemEditModel model)
        {
            try
            {
                var payingItem = await _payingItemService.GetItemAsync(model.PayingItem.ItemID);
                payingItem.PaiyngItemProducts.Clear();

                var itemsToAdd = model.PricesAndIdsInItem.Where(i => i.Id != 0).Select(i => new PaiyngItemProduct()
                {
                    ProductID = i.Id,
                    Summ = i.Price,
                    PayingItemID = payingItem.ItemID
                });

                foreach (var item in itemsToAdd)
                {
                    payingItem.PaiyngItemProducts.Add(item);
                }

                await _payingItemService.SaveAsync();

            }
            catch (ServiceException e)
            {
                throw new WebUiHelperException(
                    $"Ошибка в типе {nameof(PayingItemHelper)} в методе {nameof(CreatePayingItemProducts)}", e);
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
                model.PayingItemProductsCount = payingItemProducts.Count;

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
                    $"Ошибка в типе {nameof(PayingItemHelper)} в методе {nameof(FillPayingItemEditModel)}", e);
            }

            catch (Exception e)
            {
                throw new WebUiHelperException(
                    $"Ошибка в типе {nameof(PayingItemHelper)} в методе {nameof(FillPayingItemEditModel)}", e);
            }
        }

        public async Task UpdatePayingItemProducts(PayingItemEditModel model)
        {
            try
            {
                var payingItemProducts = _payingItemProductService.GetList(x => x.PayingItemID == model.PayingItem.ItemID).ToList();

                foreach (var item in payingItemProducts)
                {
                    await _payingItemProductService.DeleteAsync(item.ItemID);
                }

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
                    $"Ошибка в типе {nameof(PayingItemHelper)} в методе {nameof(UpdatePayingItemProducts)}", e);
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

        public void SetSumForPayingItem(PayingItemEditModel model)
        {
            var sum = 0M;

            if (model.PricesAndIdsNotInItem == null)
            {
                sum = model.PricesAndIdsInItem.Where(x => x.Id != 0).Sum(x => x.Price);
            }

            sum = model.PricesAndIdsInItem.Where(x => x.Id != 0).Sum(x => x.Price) +
                   model.PricesAndIdsNotInItem.Where(x => x.Id != 0).Sum(x => x.Price);

            if (sum != 0)
            {
                model.PayingItem.Summ = sum;
            }
        }

        private void CreateCommentWhileAdd(PayingItemModel model)
        {
            if (string.IsNullOrEmpty(model.PayingItem.Comment))
            {                
                try
                {
                    var comment = string.Empty;

                    foreach (var item in model.Products.Where(p => p.ProductID != 0))
                    {
                        comment += item.ProductName + ", ";
                    }

                    model.PayingItem.Comment = string.IsNullOrEmpty(comment) ? comment : comment.Remove(comment.LastIndexOf(","));
                }
                catch (Exception e)
                {
                    throw new WebUiHelperException(
                        $"Ошибка в типе {nameof(PayingItemHelper)} в методе {nameof(CreateCommentWhileAdd)}", e);
                }                
            }
        }
    }
}