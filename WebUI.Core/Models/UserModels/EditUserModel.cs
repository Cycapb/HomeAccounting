using System.ComponentModel.DataAnnotations;

namespace WebUI.Core.Models.UserModels
{
    public class EditUserModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Не указан Email")]
        public string Email { get; set; }

        [UIHint("password")]
        public string Password { get; set; }
    }
}
