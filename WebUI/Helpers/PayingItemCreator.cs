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
    public class PayingItemCreator : IPayingItemCreator
    {
        private readonly IPayingItemService _payingItemService;

        public PayingItemCreator(IPayingItemService payingItemService)
        {
            _payingItemService = payingItemService;
        }

        public async Task CreatePayingItemFromViewModel(PayingItemViewModel model)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            try
            {
                if (model.PayingItem.Date.Month > DateTime.Today.Date.Month ||
                    model.PayingItem.Date.Year > DateTime.Today.Year)
                {
                    model.PayingItem.Date = DateTime.Today.Date;
                }

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
                throw new WebUiException($"Ошибка в типе {nameof(PayingItemCreator)} в методе {nameof(CreatePayingItemFromViewModel)}", e);
            }
            catch(Exception e)
            {
                throw new WebUiException($"Ошибка в типе {nameof(PayingItemCreator)} в методе {nameof(CreatePayingItemFromViewModel)}", e);
            }
        }

        private void CreateCommentWhileAdd(PayingItemViewModel model)
        {
            if (string.IsNullOrEmpty(model.PayingItem.Comment))
            {
                var comment = string.Empty;

                foreach (var item in model.Products.Where(p => p.ProductID != 0))
                {
                    comment += item.ProductName + ", ";
                }

                if (!string.IsNullOrEmpty(comment))
                {
                    model.PayingItem.Comment = comment.Remove(comment.LastIndexOf(",", StringComparison.Ordinal));
                }
            }
        }
    }
}