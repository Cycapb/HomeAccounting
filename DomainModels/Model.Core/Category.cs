using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModels.Model.Core
{
    [Table("Category")]
    public partial class Category
    {
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

        public virtual ICollection<PayingItem> PayingItems { get; set; }

        public virtual ICollection<PlanItem> PlanItems { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        public virtual TypeOfFlow TypeOfFlow { get; set; }
    }
}
