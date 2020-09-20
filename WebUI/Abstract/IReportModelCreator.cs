using System;
using WebUI.Models;
using WebUI.Models.ReportModels;

namespace WebUI.Abstract
{
    public interface IReportModelCreator : IDisposable
    {
        ReportModel CreateByDatesReportModel(WebUser user, DateTime dtFrom, DateTime dtTo, int page);
        ReportModel CreateByTypeReportModel(ReportByCategoryAndTypeOfFlowModel model, WebUser user, int page);
    }
}
