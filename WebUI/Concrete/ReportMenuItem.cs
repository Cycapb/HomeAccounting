using WebUI.Abstract;

namespace WebUI.Concrete
{
    public class ReportMenuItem:IReportMenuItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}