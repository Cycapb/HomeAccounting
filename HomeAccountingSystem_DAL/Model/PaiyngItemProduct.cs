namespace HomeAccountingSystem_DAL.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PaiyngItemProduct")]
    public partial class PaiyngItemProduct
    {
        [Key]
        public int ItemID { get; set; }

        public int ProductID { get; set; }

        public decimal Summ { get; set; }

        public int PayingItemID { get; set; }

        public virtual PayingItem PayingItem { get; set; }

        public virtual Product Product { get; set; }
    }
}
