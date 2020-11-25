namespace DomainModels.Model
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Account")]
    public partial class Account
    {
        public Account()
        {
            Debts = new HashSet<Debt>();
            PayingItems = new HashSet<PayingItem>();
        }

        public int AccountID { get; set; }

        [Required(ErrorMessage = "Необходимо указать название счета")]
        [StringLength(50)]
        public string AccountName { get; set; }

        [Column(TypeName = "money")]
        [Required(ErrorMessage = "Необходимо указать сумму на счете")]
        [DataType(DataType.Currency)]
        [Display(Name = "Сумма")]
        public decimal Cash { get; set; }

        public bool Use { get; set; }

        [Required]
        [StringLength(50)]
        public string UserId { get; set; }

        public virtual ICollection<Debt> Debts { get; set; }

        public virtual ICollection<PayingItem> PayingItems { get; set; }
    }
}
