using System.Collections.Generic;

namespace WebUI.Core.Abstract
{
    public interface IReportMenu
    {
        List<IReportMenuItem> Items { get; set; }
    }
}
