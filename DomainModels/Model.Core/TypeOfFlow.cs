using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModels.Model.Core
{
    [Table("TypeOfFlow")]
    public partial class TypeOfFlow
    {
        public TypeOfFlow()
        {
            Categories = new HashSet<Category>();
            Debts = new HashSet<Debt>();
        }
        
        [Key]
        public int TypeID { get; set; }

        [Required]
        [StringLength(50)]
        public string TypeName { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
        
        public virtual ICollection<Debt> Debts { get; set; }
    }
}
