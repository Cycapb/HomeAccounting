using DomainModels.Model;
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
    public class PayingItemEditViewModelCreator : IPayingItemEditViewModelCreator
    {
        private readonly IPayingItemService _payingItemService;

        public PayingItemEditViewModelCreator(IPayingItemService payingItemService)
        {
            _payingItemService = payingItemService;
        }

        public async Task<PayingItemEditViewModel> CreateViewModel(int payingItemId)
        {
            try
            {
                var payingItem = await _payingItemService.GetItemAsync(payingItemId);
                var model = new PayingItemEditViewModel()
                {
                    PayingItem = payingItem,
                    ProductsInItem = new List<Product>(),
                    ProductsNotInItem = new List<Product>()
                };
                var productsInItem = payingItem.PaiyngItemProducts.Select(x => x.Product).ToList();
                var productsByCategory = payingItem.Category.Products.ToList();                

                if (productsInItem.Count != 0)
                {
                    model.ProductsInItem = productsInItem;
                    model.ProductsNotInItem = productsByCategory.Except(productsInItem).ToList();
                }
                else
                {
                    model.ProductsNotInItem = productsByCategory;
                }

                return model;
            }
            catch (ServiceException e)
            {
                throw new WebUiHelperException(
                    $"Ошибка в типе {nameof(PayingItemHelper)} в методе {nameof(CreateViewModel)}", e);
            }

            catch (Exception e)
            {
                throw new WebUiHelperException(
                    $"Ошибка в типе {nameof(PayingItemHelper)} в методе {nameof(CreateViewModel)}", e);
            }
        }
    }
}