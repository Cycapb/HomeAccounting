using System.ComponentModel.DataAnnotations;

namespace WebUI.Core.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Не указан логин")]
        [Display(Name = "Логин")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        [Display(Name = "Пароль")]
        [UIHint("password")]
        public string Password { get; set; }
    }
}
