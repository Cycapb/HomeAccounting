using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebUI.Core.Models.MailboxModels
{
    public abstract class NotificationMailboxModelBase
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Не задано Имя")]
        [StringLength(50)]
        public abstract string MailBoxName { get; set; }

        [Required(ErrorMessage = "Не указано От кого")]
        [StringLength(50)]
        public string MailFrom { get; set; }

        [Required(ErrorMessage = "Не указан логин")]
        [StringLength(50)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        [StringLength(1024)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Подтвердите пароль")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [StringLength(1024)]
        public string PasswordConfirmation { get; set; }

        [Required(ErrorMessage = "Не указана smtp-сервер")]
        [StringLength(50)]
        public string Server { get; set; }

        [Required(ErrorMessage = "Не указан порт")]
        public int Port { get; set; }

        public bool UseSsl { get; set; } = true;
    }
}
