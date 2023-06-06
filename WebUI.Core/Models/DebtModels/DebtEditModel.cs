using DomainModels.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebUI.Core.Models.DebtModels
{
    public class DebtEditModel
    {
        [HiddenInput(DisplayValue = false)]
        public int DebtId { get; set; }

        [Required(ErrorMessage = "Не указана сумма")]
        [DataType(DataType.Currency)]
        public decimal? Sum { get; set; }

        public string Person { get; set; }

        public int AccountId { get; set; }

        public IEnumerable<Account> Accounts { get; set; }

        public string Date { get; set; }

        public int TypeOfFlowId { get; set; }
    }
}