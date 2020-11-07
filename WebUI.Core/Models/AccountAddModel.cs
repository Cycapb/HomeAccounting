﻿using System.ComponentModel.DataAnnotations;

namespace WebUI.Core.Models
{
    public class AccountAddModel
    {
        [Required(ErrorMessage = "Необходимо ввести название счета")]
        [StringLength(50)]
        public string AccountName { get; set; }

        [DataType(DataType.Currency)]
        public decimal Cash { get; set; }

        public bool Use { get; set; }
    }
}