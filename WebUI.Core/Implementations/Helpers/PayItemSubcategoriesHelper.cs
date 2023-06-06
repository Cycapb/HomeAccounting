using Services;
using Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Core.Abstract.Helpers;
using WebUI.Core.Exceptions;
using WebUI.Core.Models.CategoryModels;
using WebUI.Core.Models.ReportModels;

namespace WebUI.Core.Implementations.Helpers
{
    public class PayItemSubcategoriesHelper : IPayItemSubcategoriesHelper
    {
        private readonly IPayingItemService _payingItemService;
        private bool _disposed;

        public PayItemSubcategoriesHelper(IPayingItemService payingItemService)
        {
            _payingItemService = payingItemService;
        }

        public async Task<List<PayItemSubcategories>> GetPayItemsWithSubcategoriesInDatesWeb(DateTime dateFrom, DateTime dateTo,
            string userId, int typeOfFlowId)
        {
            try
            {
                var pItems = _payingItemService.GetList(x => x.UserId == userId && x.Date >= dateFrom.Date && (x.Date <= dateTo.Date) && x.Category.TypeOfFlowID == typeOfFlowId)
                    .ToList();

                var payItemSubcategoriesList = new List<PayItemSubcategories>();

                var ids = pItems.GroupBy(x => x.CategoryID) //Список ИД категорий за выбранный период
                    .ToList();
                ids.ForEach(id => payItemSubcategoriesList.Add(new PayItemSubcategories() //Наполняем у ViewModel свойства CategoryId
                {
                    CategoryId = id.Key,
                    CategorySumm = new CategorySumModel(),
                    ProductPrices = new List<ProductPrice>()
                }));

                var catNameGrouping = pItems //Наполняем список View Model и заполняем у каждого итема свойство CategorySumm
                    .Where(x => x.UserId == userId &&
                                ((x.Date >= dateFrom.Date) && (x.Date <= dateTo.Date))
                                && x.Category.TypeOfFlowID == typeOfFlowId)
                    .GroupBy(x => x.Category.Name)
                    .ToList();

                var i = 0;
                foreach (var item in payItemSubcategoriesList)
                {
                    item.CategorySumm.Category = catNameGrouping[i].Key;
                    item.CategorySumm.Sum = catNameGrouping[i].Sum(x => x.Summ);
                    i++;
                }

                foreach (var item in payItemSubcategoriesList)
                {
                    item.ProductPrices = await FillProductPrices(item.CategoryId, dateFrom, dateTo);
                }
                return payItemSubcategoriesList;
            }
            catch (ServiceException e)
            {
                throw new WebUiHelperException($"Ошибка в сервисе {nameof(PayItemSubcategoriesHelper)} в методе {nameof(GetPayItemsWithSubcategoriesInDatesWeb)} при обращении к БД", e);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _payingItemService.Dispose();
                }

                _disposed = true;
            }
        }

        private async Task<List<ProductPrice>> FillProductPrices(int catId, DateTime dateFrom, DateTime dateTo)
        {
            var payingItems = await _payingItemService
                .GetListAsync(x => x.CategoryID == catId && x.Date >= dateFrom.Date && x.Date <= dateTo.Date)
                .ConfigureAwait(false);
            var productPrices = payingItems.ToList()
                .SelectMany(x => x.PayingItemProducts)
                .GroupBy(x => x.Product.ProductName)
                .Select(x => new ProductPrice() { ProductName = x.Key, Price = x.Sum(p => p.Price) })
                .ToList();

            return productPrices;
        }

    }
}