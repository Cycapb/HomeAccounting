using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DomainModels.Model;
using WebUI.Core.Infrastructure.Attributes;

namespace WebUI.Core.Models
{
    public class TransferModel
    {
        public List<Account> FromAccounts { get; set; }

        public List<Account> ToAccounts { get; set; }

        [TransferModel(ErrorMessage = "Необходимо завести хотя бы один счет")]
        [Display(Name = "Исходный счет")]
        public int FromId { get; set; }

        [Display(Name = "Конечный счет")]
        public int ToId { get; set; }

        [Display(Name = "Сумма для перевода")]
        [Required(ErrorMessage = "Введите сумму для перевода")]
        [RegularExpression(@"\d+(,)?\d+",ErrorMessage = "Некорректно введена сумма")]
        public string Summ { get; set; }
    }
}