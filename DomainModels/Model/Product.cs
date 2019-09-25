namespace DomainModels.Model
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Product")]
    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            OrderDetails = new HashSet<OrderDetail>();
            PaiyngItemProducts = new HashSet<PaiyngItemProduct>();
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PaiyngItemProduct> PaiyngItemProducts { get; set; }     
        public virtual ICollection<PayingItemProduct> PayingItemProducts { get; set; }
    }
}
