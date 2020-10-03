using System.ComponentModel.DataAnnotations;

namespace WebUI.Core.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Не указан логин")]
        [Display(Name = "Логин")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Не указан email")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        [Display(Name = "Пароль")]
        [UIHint("password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Не указано подтверждение пароля")]
        [Compare("Password", ErrorMessage = "Не совпадает пароль и подтверждение")]
        [Display(Name = "Подтверждение пароля")]
        [UIHint("password")]
        public string ConfirmPassword { get; set; }
    }
}
