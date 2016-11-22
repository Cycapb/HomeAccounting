using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace HomeAccountingSystem_DAL.Model
{
    [MetadataType(typeof(ProductMetaData))]
    public partial class Product
    {
        [NotMapped]
        public decimal Price { get; set; }
    }

    public class ProductMetaData
    {
        [HiddenInput(DisplayValue = false)]
        public int ProductID { get; set; }

        [Required(ErrorMessage = "Необходимо ввести наименование")]
        [Display(Name = "Наименование")]
        public string ProductName { get; set; }
    }
}
