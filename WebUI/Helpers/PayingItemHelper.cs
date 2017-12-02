using System.Collections.Generic;
using System.Linq;
using DomainModels.Model;
using WebUI.Abstract;
using WebUI.Models;
using Services;
using Services.Exceptions;
using WebUI.Exceptions;

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
                var products = new List<Product>();
                try
                {
                    products =
                        _productService.GetList().Where(x => x.CategoryID == model.PayingItem.CategoryID).ToList();
                }
                catch (ServiceException e)
                {
                    throw new WebUiHelperException(
                        $"Ошибка в типе {nameof(PayingItemHelper)} в методе {nameof(CreateCommentWhileAdd)}", e);
                }
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
            var products = new List<Product>();
            try
            {
                products = _productService.GetList().Where(x => x.CategoryID == model.PayingItem.CategoryID).ToList();
            }
            catch (ServiceException e)
            {
                throw new WebUiHelperException(
                    $"Ошибка в типе {nameof(PayingItemHelper)} в методе {nameof(CreateCommentWhileEdit)}", e);
            }
            
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