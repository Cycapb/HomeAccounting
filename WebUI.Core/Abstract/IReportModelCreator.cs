using System;
using WebUI.Core.Models;
using WebUI.Core.Models.ReportModels;

namespace WebUI.Core.Abstract
{
    public interface IReportModelCreator : IDisposable
    {
        ReportModel CreateByDatesReportModel(WebUser user, DateTime dtFrom, DateTime dtTo, int page);

        ReportModel CreateByTypeReportModel(ReportByCategoryAndTypeOfFlowModel model, WebUser user, int page);
    }
}
