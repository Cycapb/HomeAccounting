using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace HomeAccountingSystem_DAL.Model
{
    [MetadataType(typeof(AccountMetaData))]
    public partial class Account
    {

    }

    public class AccountMetaData
    {
         [HiddenInput(DisplayValue = false)]
         public int AccountID { get; set; }

        [Required(ErrorMessage = "Необходимо указать название счета")]
        public string AccountName { get; set; }

        [Required(ErrorMessage = "Необходимо указать сумму на счете")]
        [DataType(DataType.Currency)]
        [Display(Name = "Сумма")]
        public decimal Cash { get; set; }
    }
}
