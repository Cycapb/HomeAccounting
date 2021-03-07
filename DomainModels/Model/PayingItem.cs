namespace DomainModels.Model
{
    using DomainModels.Infrastructure;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PayingItem")]
    public partial class PayingItem
    {
        public PayingItem()
        {
            PayingItemProducts = new HashSet<PayingItemProduct>();
        }

        [Key]
        public int ItemID { get; set; }

        [CategoryModel(ErrorMessage = "Необходимо завести хотя бы одну категорию")]
        public int CategoryID { get; set; }

        [Column(TypeName = "money")]
        [Required(ErrorMessage = "Необходимо ввести сумму")]
        [Display(Name = "Сумма")]
        [DataType(DataType.Currency)]
        public decimal Summ { get; set; }

        [UIHint("text")]
        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Необходимо указать дату")]
        public DateTime Date { get; set; }

        [AccountModel(ErrorMessage = "Необходимо завести хотя бы один счет")]
        public int AccountID { get; set; }

        [StringLength(1024)]
        public string Comment { get; set; }

        [Required]
        [StringLength(50)]        
        public string UserId { get; set; }

        public virtual Account Account { get; set; }

        public virtual Category Category { get; set; }
                        
        public virtual ICollection<PayingItemProduct> PayingItemProducts { get; set; }
    }
}
