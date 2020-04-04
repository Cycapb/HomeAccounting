using System;

namespace WebUI.Models.ReportModels
{
    public class ReportByCategoryAndTypeOfFlowModel
    {
        public int CatId { get; set; }

        public int TypeOfFlowId { get; set; }

        public DateTime DtFrom { get; set; }

        public DateTime DtTo { get; set; }
    }
}