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
                var model = new PayingItemEditViewModel();
                var payingItemProducts = payingItem.PaiyngItemProducts;
                var productsByCategory = payingItem.Category.Products.ToList();
                    
                model.PayingItemProductsCount = payingItemProducts.Count;

                if (payingItemProducts.Count != 0)
                {
                    var productsInItem = payingItemProducts.Join(productsByCategory,
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

                    var productsNotInItem = payingItemProducts.Join(productsByCategory,
                            x => x.ProductID,
                            y => y.ProductID,
                            (x, y) => y)
                        .OrderBy(x => x.ProductName)
                        .ToList();

                    model.ProductsInItem = productsInItem;
                    model.ProductsNotInItem = productsByCategory.Except(productsNotInItem).ToList();
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