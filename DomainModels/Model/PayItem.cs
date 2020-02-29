using System;

namespace DomainModels.Model
{
    /// <summary>
    /// Is used for Reporting
    /// </summary>
    public class PayItem
    {
        public int ItemId { get; set; }
        public decimal Summ { get; set; }
        public DateTime Date { get; set; }
        public string CategoryName { get; set; }
        public string AccountName { get; set; }
        public string Comment { get; set; }
        public int TypeOfFlowId { get; set; }
    }
}
