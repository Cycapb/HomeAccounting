using DomainModels.Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebUI.Models.DebtViewModels
{
    public class DebtAddingViewModel
    {
        [Required(ErrorMessage = "Не указано поле Кому/Кто")]
        public string Person { get; set; }

        [Required(ErrorMessage = "Не указана сумма")]
        public decimal Summ { get; set; }

        public IEnumerable<Account> Accounts { get; set; }

        public List<FlowTypeForDebtViewModels> TypesOfFlow => new List<FlowTypeForDebtViewModels>()
        {
            new FlowTypeForDebtViewModels() {TypeId = 1,Name = "Я должен"},
            new FlowTypeForDebtViewModels() {TypeId = 2,Name = "Мне должны"}
        };

        public int AccountId { get; set; }

        public int TypeOfFlowId { get; set; }
    }
}