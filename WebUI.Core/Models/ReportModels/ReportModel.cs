using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebUI.Core.Models.CategoryModels;

namespace WebUI.Core.Models.ReportModels
{
    public class ReportModel
    {
        public List<PayItem> ItemsPerPage { get; set; }

        public List<PayItem> AllItems { get; set; }

        public PagingInfo PagingInfo;

        public string CategoryName { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public DateTime DtFrom { get; set; }

        public DateTime DtTo { get; set; }
    }

    public class ReportOverallLastYearByMonthsModel
    {
        public List<MonthInOut> MonthInOuts { get; set; }
    }

    public class MonthSumm
    {
        public string Date { get; set; }

        public decimal Summ { get; set; }
    }

    public class MonthInOut
    {
        public string Month { get; set; }

        public string SummIn { get; set; }

        public string SummOut { get; set; }

        public DateTime Date { get; set; }
    }

    public class PayItemSubcategories
    {
        public int CategoryId { get; set; }

        public CategorySumModel CategorySumm { get; set; }

        public List<ProductPrice> ProductPrices { get; set; }
    }

    public class ProductPrice
    {
        public string ProductName { get; set; }

        public decimal Price { get; set; }
    }
}