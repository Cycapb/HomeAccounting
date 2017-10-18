using System.Linq;
using WebUI.Abstract;
using WebUI.Models;
using Services;

namespace WebUI.Helpers
{
    public class PayingItemHelper:IPayingItemHelper
    {
        private readonly IProductService _productService;

        public PayingItemHelper(IProductService productService)
        {
            _productService = productService;
        }

        public void CreateCommentWhileAdd(PayingItemModel model)
        {
            if (string.IsNullOrEmpty(model.PayingItem.Comment))
            {
                var products =
                    _productService.GetList().Where(x => x.CategoryID == model.PayingItem.CategoryID).ToList();
                var comment = string.Empty;
                foreach (var item in model.Products)
                {
                    if (item.ProductID != 0)
                    {
                        comment += products.Single(x => x.ProductID == item.ProductID).ProductName + ", ";
                    }
                }
                model.PayingItem.Comment = string.IsNullOrEmpty(comment)?comment : comment.Remove(comment.LastIndexOf(","));
            }
        }

        public void CreateCommentWhileEdit(PayingItemEditModel model)
        {
            var products = _productService.GetList().Where(x => x.CategoryID == model.PayingItem.CategoryID).ToList();
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
    }
}