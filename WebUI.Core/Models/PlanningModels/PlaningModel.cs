using DomainModels.Model;
using System.Collections.Generic;

namespace WebUI.Core.Models.PlanningModels
{
    public class PlanningModel
    {
        public List<string> Months { get; set; }

        public List<Category> CategoryPlanItemsIncome { get; set; }

        public List<Category> CategoryPlanItemsOutgo { get; set; }

        public List<PlanItem> PlanItemsIncomePlan { get; set; }

        public List<PlanItem> PlanItemsOutgoPlan { get; set; }

        public List<BalanceModel> Balances { get; set; }
    }
}