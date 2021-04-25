using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebUI.Core.Infrastructure.Identity.Models;

namespace WebUI.Core.Models
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

    public class RoleEditModel
    {
        public AccRoleModel Role { get; set; }
        public IEnumerable<AccountingUserModel> Members { get; set; } 
        public IEnumerable<AccountingUserModel> NonMembers { get; set; } 
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

    public class EditUserModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Не указан Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        [UIHint("password")]
        public string Password { get; set; }
    }

}