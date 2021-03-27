using System.Collections.Generic;

namespace WebUI.Core.Models.ReportModels
{
    public class ReportMenu
    {
        public List<ReportMenuItem> Items => new List<ReportMenuItem>()
        {
            new ReportMenuItem() {Id = 1,Name = "Доход"},
            new ReportMenuItem() {Id = 2,Name = "Расход"},
            new ReportMenuItem() {Id = 3,Name = "По датам"},
        };
    }
}