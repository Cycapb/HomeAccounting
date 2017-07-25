using System;
using System.ComponentModel.DataAnnotations;

namespace DomainModels.Model
{
    [MetadataType(typeof(DebtMetadata))]
    public partial class Debt
    {
    }

    public class DebtMetadata
    {        
        public int DebtID { get; set; }

        [Required]
        public Nullable<int> TypeOfFlowId { get; set; }

        [Required]
        public Nullable<int> AccountId { get; set; }

        [DataType(DataType.Currency)]
        public decimal Summ { get; set; }

        [Required(ErrorMessage = "Не задано поле Кому/Кто")]
        public string Person { get; set; }

        [Required(ErrorMessage = "Не выбрана дата займа")]
        public System.DateTime DateBegin { get; set; }
        
        public string UserId { get; set; }
    }
}
