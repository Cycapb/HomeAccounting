using System.Collections.Generic;
using WebUI.Abstract;

namespace WebUI.Concrete
{
    public class ReportMenu : IReportMenu
    {
        public List<IReportMenuItem> Items { get; set; }

        public ReportMenu()
        {
            Items = new List<IReportMenuItem>();
            CreateItemList();
        }

        private void CreateItemList()
        {
            IEnumerable<ReportMenuItem> list = new List<ReportMenuItem>()
            {
                new ReportMenuItem() {Id = 1,Name = "Доход"},
                new ReportMenuItem() {Id = 2,Name = "Расход"},
                new ReportMenuItem() {Id = 3,Name = "По датам"},
            };
            Items.AddRange(list);
        }
    }
}