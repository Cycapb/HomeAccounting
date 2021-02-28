using System.ComponentModel.DataAnnotations;

namespace WebUI.Core.Models
{
    public class AccountAddModel
    {
        [Required(ErrorMessage = "Необходимо ввести название счета")]
        [StringLength(50)]
        [Display(Name = "Наименование счета")]
        public string AccountName { get; set; }
                
        [Display(Name = "Сумма на счету")]
        [Required(ErrorMessage = "Необходимо ввести сумму")]
        public decimal? Cash { get; set; }

        [Display(Name = "Отображать в текущем бюджете")]
        public bool Use { get; set; }
    }
}