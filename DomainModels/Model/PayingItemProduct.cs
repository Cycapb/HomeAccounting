using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModels.Model
{
    [Table("PayingItemProduct")]
    public class PayingItemProduct
    {
        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Key]
        [Column(Order = 2)]
        public int PayingItemId { get; set; }

        public int? ProductId { get; set; }

        public decimal Price { get; set; }

        [ForeignKey("PayingItemId")]
        public virtual PayingItem PayingItem { get; set; }

        public virtual Product Product { get; set; }
    }
}
