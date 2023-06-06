using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace WebUI.Core.Models.UserModels
{
    public class ChangeUserPasswordModel
{
        [Required(ErrorMessage = "Не указан текущий пароль")]
        [Display(Name = "Текущий пароль")]
        [UIHint("password")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Не указан новый пароль")]
        [Display(Name = "Новый пароль")]
        [UIHint("password")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Не подтвержден новый пароль пароль")]
        [Compare("NewPassword", ErrorMessage = "Не совпадает новый пароль и подтверждение")]
        [Display(Name = "Подтверждение пароля")]
        [UIHint("password")]
        public string ConfirmPassword { get; set; }
    }
}
