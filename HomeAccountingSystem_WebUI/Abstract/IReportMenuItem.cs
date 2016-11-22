using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAccountingSystem_WebUI.Abstract
{
    public interface IReportMenuItem
    {
        int Id { get; set; }
        string Name { get; set; }
    }
}
