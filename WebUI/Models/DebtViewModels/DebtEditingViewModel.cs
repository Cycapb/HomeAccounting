using DomainModels.Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace WebUI.Models.DebtViewModels
{
    public class DebtEditingViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int DebtId { get; set; }

        [Required(ErrorMessage = "Не указана сумма")]
        [DataType(DataType.Currency)]
        public decimal Sum { get; set; }

        public string Person { get; set; }

        public int AccountId { get; set; }

        public IEnumerable<Account> Accounts { get; set; }

        public string Date { get; set; }

        public int TypeOfFlowId { get; set; }
    }
}