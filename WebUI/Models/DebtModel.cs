using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using DomainModels.Model;

namespace WebUI.Models
{
    public class FlowType
    {
        public int TypeId { get; set; }
        public string Name { get; set; }
    }

    public class DebtsSumModel
    {
        public decimal MyDebtsSumm { get; set; } 
        public decimal DebtsToMeSumm { get; set; }
    }

    public class DebtsModel
    {
        public IEnumerable<Debt> MyDebts { get; set; }
        public IEnumerable<Debt> DebtsToMe { get; set; }
    }

    public class DebtsAddModel
    {
        [Required(ErrorMessage = "Не указано поле Кому/Кто")]
        public string Person { get; set; }

        [Required(ErrorMessage = "Не указана сумма")]
        public decimal Summ { get; set; }

        public IEnumerable<Account> Accounts { get; set; }

        public List<FlowType> TypesOfFlow => new List<FlowType>()
        {
            new FlowType() {TypeId = 1,Name = "Я должен"},
            new FlowType() {TypeId = 2,Name = "Мне должны"}
        };

        public int AccountId { get; set; }

        public int TypeOfFlowId { get; set; }
    }

    public class DebtEditViewModel
    {

        [HiddenInput(DisplayValue = false)]
        public int DebtId { get; set; }

        [Required(ErrorMessage = "Не указана сумма")]
        [DataType(DataType.Currency)]
        public decimal Sum { get; set; }

        public string Person { get; set; }

        public string AccountName { get; set; }

        public string Date { get; set; }
    }
}