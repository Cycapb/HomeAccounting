namespace DomainModels.Model
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

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
