using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DomainModels.Model
{
    [MetadataType(typeof(CategoryMetaData))]
    public partial class Category
    {

    }

    public class CategoryMetaData
    {
        [HiddenInput(DisplayValue = false)]
        public int CategoryID { get; set; }

        [Required(ErrorMessage = "Необходимо задать имя категории")]
        public string Name { get; set; }
    }
}
