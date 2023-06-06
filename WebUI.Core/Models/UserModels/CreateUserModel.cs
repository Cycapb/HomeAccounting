using System.ComponentModel.DataAnnotations;

namespace WebUI.Core.Models.UserModels
{
    public class CreateUserModel
    {
        [Required(ErrorMessage = "Не указано имя")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Не указан Email")]
        public string Email { get; set; }

        [UIHint("password")]
        [Required(ErrorMessage = "Не указан пароль")]
        public string Password { get; set; }
    }
}
