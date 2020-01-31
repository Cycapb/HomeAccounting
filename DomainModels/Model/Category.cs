namespace DomainModels.Model
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Category")]
    public partial class Category
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Category()
        {
            PayingItems = new HashSet<PayingItem>();
            PlanItems = new HashSet<PlanItem>();
            Products = new HashSet<Product>();
        }

        public int CategoryID { get; set; }

        [Required(ErrorMessage = "Необходимо задать имя категории")]
        [StringLength(50)]
        public string Name { get; set; }

        public int TypeOfFlowID { get; set; }

        [Required]
        [StringLength(50)]
        public string UserId { get; set; }

        public bool ViewInPlan { get; set; }
        public bool Active { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PayingItem> PayingItems { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PlanItem> PlanItems { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product> Products { get; set; }

        public virtual TypeOfFlow TypeOfFlow { get; set; }
    }
}
