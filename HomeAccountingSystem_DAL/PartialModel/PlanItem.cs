using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DomainModels.Model
{
    [MetadataType(typeof(PlanItemMetaData))]
    public partial class PlanItem
    {
        
    }

    public class PlanItemMetaData
    {
        [HiddenInput(DisplayValue= false)]
        public int PlanItemID { get; set; }
        [DataType(DataType.Currency)]
        public decimal SummPlan { get; set; }
    }
}
