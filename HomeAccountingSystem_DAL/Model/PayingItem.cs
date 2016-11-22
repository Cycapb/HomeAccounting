namespace HomeAccountingSystem_DAL.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayingItem")]
    public partial class PayingItem
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PayingItem()
        {
            PaiyngItemProduct = new HashSet<PaiyngItemProduct>();
        }

        [Key]
        public int ItemID { get; set; }

        public int CategoryID { get; set; }

        [Column(TypeName = "money")]
        public decimal Summ { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        public int AccountID { get; set; }

        [StringLength(1024)]
        public string Comment { get; set; }

        [Required]
        [StringLength(50)]
        public string UserId { get; set; }

        public virtual Account Account { get; set; }

        public virtual Category Category { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PaiyngItemProduct> PaiyngItemProduct { get; set; }
    }
}
