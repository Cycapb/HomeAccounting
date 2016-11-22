namespace HomeAccountingSystem_DAL.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Account")]
    public partial class Account
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Account()
        {
            Debt = new HashSet<Debt>();
            PayingItem = new HashSet<PayingItem>();
        }

        public int AccountID { get; set; }

        [Required]
        [StringLength(50)]
        public string AccountName { get; set; }

        [Column(TypeName = "money")]
        public decimal Cash { get; set; }

        public bool Use { get; set; }

        [Required]
        [StringLength(50)]
        public string UserId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Debt> Debt { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PayingItem> PayingItem { get; set; }
    }
}
