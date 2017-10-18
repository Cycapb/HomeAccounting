using System.ComponentModel.DataAnnotations;

namespace DomainModels.Model
{
    [MetadataType(typeof(CategoryMetaData))]
    public partial class Category
    {

    }

    public class CategoryMetaData
    {       
        public int CategoryID { get; set; }

        [Required(ErrorMessage = "Необходимо задать имя категории")]
        public string Name { get; set; }
    }
}
