using System.Collections.Generic;
using DomainModels.Model;

namespace WebUI.Models.DebtViewModels
{
    public class DebtsCollectionViewModel
    {
        public IEnumerable<Debt> MyDebts { get; set; }
        public IEnumerable<Debt> DebtsToMe { get; set; }
    }
}