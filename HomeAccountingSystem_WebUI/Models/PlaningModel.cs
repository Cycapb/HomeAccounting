using System.Collections.Generic;
using DomainModels.Model;

namespace HomeAccountingSystem_WebUI.Models
{
    public class ViewPlaningModel
    {
        public List<string> Months { get; set; }
        public List<Category> CategoryPlanItemsIncome { get; set; } 
        public List<Category> CategoryPlanItemsOutgo { get; set; } 
        public List<PlanItem> PlanItemsIncomePlan { get; set; }  
        public List<PlanItem> PlanItemsOutgoPlan { get; set; } 
        public List<BalanceModel> Balances { get; set; } 
    }

    public class BalanceModel
    {
        public decimal BalancePlan { get; set; }
        public decimal BalanceFact { get; set; }
        public decimal OstatokFact { get; set; }
        public decimal OstatokPlan { get; set; }
    }

    public class EditPlaningModel
    {
        public PlanItem PlanItem { get; set; }
        public bool Spread { get; set; }
    }

}