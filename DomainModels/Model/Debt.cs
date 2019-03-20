namespace DomainModels.Model
{
    using System;    
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;    

    [Table("Debt")]
    public partial class Debt
    {
        public int DebtID { get; set; }

        public int TypeOfFlowId { get; set; }

        public int AccountId { get; set; }

        [Column(TypeName = "money")]
        public decimal Summ { get; set; }

        [Required]
        [StringLength(200)]
        public string Person { get; set; }

        public DateTime DateBegin { get; set; }

        public DateTime? DateEnd { get; set; }

        [Required]
        [StringLength(50)]
        public string UserId { get; set; }

        public virtual Account Account { get; set; }

        public virtual TypeOfFlow TypeOfFlow { get; set; }
    }
}
