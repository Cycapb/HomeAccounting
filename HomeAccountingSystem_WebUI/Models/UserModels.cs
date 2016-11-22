using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HomeAccountingSystem_WebUI.Models
{
    public class CreateModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class LoginModel
    {
        [Required(ErrorMessage = "Не указан логин")]
        [Display(Name = "Логин")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
    }

    public class RoleEditModel
    {
        public AccRoleModel Role { get; set; }
        public IEnumerable<AccUserModel> Members { get; set; } 
        public IEnumerable<AccUserModel> NonMembers { get; set; } 
    }

    public class RoleModificationModel
    {
        public string RoleName { get; set; }
        public string[] IdsToAdd { get; set; }
        public string[] IdsToDelete { get; set; }
    }

    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "Не указан текущий пароль")]
        [Display(Name = "Текущий пароль")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Не указан новый пароль")]
        [Display(Name = "Новый пароль")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Не подтвержден новый пароль пароль")]
        [Display(Name = "Подтверждение пароля")]
        public string ConfirmPassword { get; set; }
    }

    public class RegisterModel
    {
        [Required]
        [Display(Name = "Логин")]
        public string Login { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Подтверждение пароля")]
        public string ConfirmPassword { get; set; }
    }

    public class EditModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Не указан Email")]
        public string Email { get; set; }

        public string Password { get; set; }
    }

}