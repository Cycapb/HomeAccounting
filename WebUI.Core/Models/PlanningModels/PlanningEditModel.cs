using DomainModels.Model;

namespace WebUI.Core.Models.PlanningModels
{
    public class PlanningEditModel
    {
        public PlanItem PlanItem { get; set; }

        public bool Spread { get; set; }
    }
}