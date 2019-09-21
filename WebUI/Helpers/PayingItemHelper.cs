using DomainModels.Model;
using Services;
using Services.Exceptions;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Abstract;
using WebUI.Exceptions;
using WebUI.Models.PayingItemViewModels;

namespace WebUI.Helpers
{
    public class PayingItemHelper : IPayingItemHelper
    {
        private readonly IProductService _productService;
        private readonly IPayingItemProductService _payingItemProductService;
        private readonly IPayingItemService _payingItemService;

        public PayingItemHelper(IProductService productService, IPayingItemProductService payingItemProductService,
            IPayingItemService payingItemService)
        {
            _productService = productService;
            _payingItemProductService = payingItemProductService;
            _payingItemService = payingItemService;
        }

        public async Task CreatePayingItemProducts(PayingItemEditViewModel model)
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

        public async Task UpdatePayingItemProducts(PayingItemEditViewModel model)
        {
            try
            {
                var payingItemProducts = _payingItemProductService
                    .GetList(x => x.PayingItemID == model.PayingItem.ItemID).ToList();

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
    }
}