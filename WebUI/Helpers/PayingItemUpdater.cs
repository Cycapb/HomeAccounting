using System.Threading.Tasks;
using DomainModels.Model;
using WebUI.Abstract;
using WebUI.Models.PayingItemViewModels;

namespace WebUI.Helpers
{
    public class PayingItemUpdater : IPayingItemUpdater
    {
        public Task<PayingItem> UpdatePayingItemFromViewModel(PayingItemEditViewModel model)
        {
            throw new System.NotImplementedException();
        }

        private void SetSumForPayingItem(PayingItemEditViewModel model)
        {
            //var sum = 0M;

            //if (model.PricesAndIdsNotInItem == null)
            //{
            //    sum = model.PricesAndIdsInItem.Where(x => x.Id != 0).Sum(x => x.Price);
            //}

            //sum = model.PricesAndIdsInItem.Where(x => x.Id != 0).Sum(x => x.Price) +
            //      model.PricesAndIdsNotInItem.Where(x => x.Id != 0).Sum(x => x.Price);

            //if (sum != 0)
            //{
            //    model.PayingItem.Summ = sum;
            //}
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