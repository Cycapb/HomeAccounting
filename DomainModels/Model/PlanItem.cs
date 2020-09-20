namespace DomainModels.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PlanItem")]
    public partial class PlanItem
    {
        public int PlanItemID { get; set; }

        public int CategoryId { get; set; }

        public DateTime Month { get; set; }

        [Column(TypeName = "money")]
        [DataType(DataType.Currency)]
        public decimal SummPlan { get; set; }

        [Column(TypeName = "money")]
        public decimal SummFact { get; set; }

        [Required]
        [StringLength(50)]
        public string UserId { get; set; }

        public bool Closed { get; set; }

        [Column(TypeName = "money")]
        public decimal IncomePlan { get; set; }

        [Column(TypeName = "money")]
        public decimal OutgoPlan { get; set; }

        [Column(TypeName = "money")]
        public decimal IncomeOutgoFact { get; set; }

        [Column(TypeName = "money")]
        public decimal BalanceFact { get; set; }

        [Column(TypeName = "money")]
        public decimal BalancePlan { get; set; }

        public virtual Category Category { get; set; }
    }
}
