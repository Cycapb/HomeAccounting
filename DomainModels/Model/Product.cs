namespace DomainModels.Model
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Product")]
    public partial class Product
    {
        public Product()
        {
            OrderDetails = new HashSet<OrderDetail>();            
            PayingItemProducts = new HashSet<PayingItemProduct>();
        }

        public int ProductID { get; set; }

        [Required(ErrorMessage = "Необходимо ввести наименование")]
        [Display(Name = "Наименование")]
        [StringLength(50)]
        public string ProductName { get; set; }

        public int CategoryID { get; set; }

        [Required]
        [StringLength(50)]
        public string UserID { get; set; }

        [StringLength(50)]
        public string Description { get; set; }

        [NotMapped]
        public decimal Price { get; set; }

        public virtual Category Category { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        public virtual ICollection<PayingItemProduct> PayingItemProducts { get; set; }
    }
}
