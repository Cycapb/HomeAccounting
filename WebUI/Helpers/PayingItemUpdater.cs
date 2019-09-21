using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModels.Model;
using Services;
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
            var sum = GetSumFromProducts(model);
            var payingItem = await _payingItemService.GetItemAsync(model.PayingItem.ItemID);
            payingItem.Summ = sum == 0 ? payingItem.Summ : sum;

            return payingItem;
        }

        private decimal GetSumFromProducts(PayingItemEditViewModel model)
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

        private void CreateCommentWhileEdit(PayingItemEditViewModel model)
        {
            //try
            //{
            //    var products = _productService.GetList(x => x.CategoryID == model.PayingItem.CategoryID).ToList();
            //    model.PayingItem.Comment = "";
            //    var comment = string.Empty;

            //    foreach (var item in model.PricesAndIdsInItem)
            //    {
            //        if (item.Id != 0)
            //        {
            //            comment += products.Single(x => x.ProductID == item.Id).ProductName + ", ";
            //        }
            //    }

            //    if (model.PricesAndIdsNotInItem != null)
            //    {
            //        foreach (var item in model.PricesAndIdsNotInItem)
            //        {
            //            if (item.Id != 0)
            //            {
            //                comment += products.Single(x => x.ProductID == item.Id).ProductName + ", ";
            //            }
            //        }
            //    }

            //    if (!string.IsNullOrEmpty(comment))
            //    {
            //        model.PayingItem.Comment = comment.Remove(comment.LastIndexOf(",", StringComparison.Ordinal));
            //    }
            //}
            //catch (ServiceException e)
            //{
            //    throw new WebUiHelperException(
            //        $"Ошибка в типе {nameof(PayingItemHelper)} в методе {nameof(CreateCommentWhileEdit)}", e);
            //}
        }
    }
}