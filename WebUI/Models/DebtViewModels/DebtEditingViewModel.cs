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

        public string AccountName { get; set; }

        public string Date { get; set; }

        public int TypeOfFlowId { get; set; }
    }
}