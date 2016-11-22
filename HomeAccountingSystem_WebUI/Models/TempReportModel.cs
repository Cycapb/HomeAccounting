using System;

namespace HomeAccountingSystem_WebUI.Models
{
    public class TempReportModel
    {
        public int CatId { get; set; }
        public int TypeOfFlowId { get; set; }
        public DateTime DtFrom { get; set; }
        public DateTime DtTo { get; set; }
    }
}