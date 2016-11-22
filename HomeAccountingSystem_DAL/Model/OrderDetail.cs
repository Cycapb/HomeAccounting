namespace HomeAccountingSystem_DAL.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OrderDetail")]
    public partial class OrderDetail
    {
        public int ID { get; set; }

        public int OrderId { get; set; }

        public int ProductId { get; set; }

        [Column(TypeName = "money")]
        public decimal? ProductPrice { get; set; }

        public int? Quantity { get; set; }

        public virtual Order Order { get; set; }

        public virtual Product Product { get; set; }
    }
}
