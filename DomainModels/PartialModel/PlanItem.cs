using System.ComponentModel.DataAnnotations;

namespace DomainModels.Model
{
    [MetadataType(typeof(PlanItemMetaData))]
    public partial class PlanItem
    {
        
    }

    public class PlanItemMetaData
    {        
        public int PlanItemID { get; set; }
        [DataType(DataType.Currency)]
        public decimal SummPlan { get; set; }
    }
}
