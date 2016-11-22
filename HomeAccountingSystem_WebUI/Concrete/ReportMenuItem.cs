using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HomeAccountingSystem_WebUI.Abstract;

namespace HomeAccountingSystem_WebUI.Concrete
{
    public class ReportMenuItem:IReportMenuItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}