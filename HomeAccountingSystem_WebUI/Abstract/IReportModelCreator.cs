using System;
using HomeAccountingSystem_WebUI.Models;

namespace HomeAccountingSystem_WebUI.Abstract
{
    public interface IReportModelCreator
    {
        ReportModel CreateByDatesReportModel(WebUser user, DateTime dtFrom, DateTime dtTo, int page);
        ReportModel CreateByTypeReportModel(TempReportModel model, WebUser user, int page);
    }
}
