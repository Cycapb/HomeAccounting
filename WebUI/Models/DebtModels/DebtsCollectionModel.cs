using DomainModels.Model;
using System.Collections.Generic;

namespace WebUI.Models.DebtModels
{
    public class DebtsCollectionModel
    {
        public IEnumerable<Debt> MyDebts { get; set; }

        public IEnumerable<Debt> DebtsToMe { get; set; }
    }
}