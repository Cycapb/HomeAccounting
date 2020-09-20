using System.Collections.Generic;

namespace WebUI.Abstract
{
    public interface IReportMenu
    {
        List<IReportMenuItem> Items { get; set; }
    }
}
