using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModels.Model.Core
{
    [Table("PayingItemProduct")]
    public class PayingItemProduct
    {
        [Key]
        public int Id { get; set; }

        public int PayingItemId { get; set; }

        public decimal Price { get; set; }

        public virtual PayingItem PayingItem { get; set; }

        public virtual Product Product { get; set; }
    }
}
