namespace DomainModels.Model
{
    using System;    
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;    

    [Table("Debt")]
    public partial class Debt
    {
        public int DebtID { get; set; }

        [Required]
        public int TypeOfFlowId { get; set; }

        [Required]
        public int AccountId { get; set; }

        [Column(TypeName = "money")]
        [DataType(DataType.Currency)]
        public decimal Summ { get; set; }

        [Required(ErrorMessage = "Не задано поле Кому/Кто")]
        [StringLength(200)]
        public string Person { get; set; }

        [Required(ErrorMessage = "Не выбрана дата займа")]
        public DateTime DateBegin { get; set; }

        public DateTime? DateEnd { get; set; }

        [Required]
        [StringLength(50)]
        public string UserId { get; set; }

        public virtual Account Account { get; set; }

        public virtual TypeOfFlow TypeOfFlow { get; set; }
    }
}
